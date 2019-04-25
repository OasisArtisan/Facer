using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Facer.FaceApi;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Facer.Data;

namespace Facer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdentificationPage : CustomContentPage
	{
        private CustomContentPage _parent;
        private Dictionary<Person, IdentificationInfo> identificationResults;
        private string tempfile;
		public IdentificationPage (CustomContentPage parent)
		{
            _parent = parent;
            InitializeComponent();
            AcceptButton.Clicked += AddRecord;
            CancelButton.Clicked += (o,e) => {
                Navigation.PopModalAsync();
            };
        }

        public async void TakeRecordImage(object o, EventArgs e)
        {
            App.Reference.Printer.PrintLine("[IdentificationPage] Adding Record");
            string response = await _parent.DisplayActionSheet(null, "Cancel", null, "Take Picture", "Choose Picture", "Create Empty Record");
            App.Reference.Printer.PrintLine($"[IdentificationPage] Received '{response}' response.");
            if (response == null || response.Length == 0 || response.Equals("Cancel"))
            {
                return;
            }
            MediaFile imgFile = null;
            if(response.Equals("Create Empty Record"))
            {
                AttendanceRecord ar = new AttendanceRecord()
                {
                    Date = DateTime.Now
                };
                App.Reference.Data.CreateAttendanceRecord(ar);
                _parent.Refresh();
                return;
            }
            else if (response.Equals("Take Picture"))
            {
                imgFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    SaveToAlbum = true,
                    Directory = "FacerAttendance",
                    Name = "class"
                });
                App.Reference.Printer.PrintLine($"[IdentificationPage] image captured. Path: {imgFile.Path}");
            }
            else if (response.Equals("Choose Picture"))
            {
                imgFile = await CrossMedia.Current.PickPhotoAsync();
                App.Reference.Printer.PrintLine($"[IdentificationPage] image chosen. Path: {imgFile.Path}");
            }
            tempfile = imgFile.Path;
            // Detect and identify people in the image
            identificationResults = await App.Reference.FaceAPI.Identify(imgFile.Path);

            // Show detection and identification results
            ImageView.Source = ImageSource.FromStream(() =>
            {
                return imgFile.GetStream();
            });
            string resultText = $"Detected {identificationResults.Count} faces.\n";
            string peopleIdentified = "";
            int identified = 0;
            foreach (Person p in identificationResults.Keys)
            {
                if (p.LocalID == null)
                {
                    continue;
                }
                peopleIdentified += $"\nID:{p.LocalID} CONF:{identificationResults[p].Confidence * 100f}%";
                identified++;
            }
            resultText += $"Identified {identified} faces.";
            resultText += peopleIdentified;
            ResultsLabel.Text = resultText;

            await _parent.Navigation.PushModalAsync(this);
        }
        public async void AddRecord(object o, EventArgs e)
        {
            AttendanceRecord ar = new AttendanceRecord()
            {
                Date = DateTime.Now
            };
            foreach (Person p in identificationResults.Keys)
            {
                if(p.LocalID == null)
                {
                    continue;
                }
                Student st = App.Reference.Data.GetEnrolledStudent(p.LocalID);
                if(st != null)
                {
                    ar.AddStudent(st);
                }
            }
            App.Reference.Data.CreateAttendanceRecord(ar);
            _parent.Refresh();
            await Navigation.PopModalAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Delete temp file
            System.IO.File.Delete(tempfile);
        }
    }
}