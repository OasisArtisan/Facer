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

		public IdentificationPage (CustomContentPage parent)
		{
            _parent = parent;
            InitializeComponent();
            AcceptButton.Clicked += AddRecord;
            RetakeButton.Clicked += (o, e) =>
            {
                Navigation.PopModalAsync(false);
                TakeRecordImage(o, e);
            };
            CancelButton.Clicked += (o,e) => {
                Navigation.PopModalAsync();
            };
        }

        public async void TakeRecordImage(object o, EventArgs e)
        {
            string response = await DisplayActionSheet(null, "Cancel", null, "Take Picture", "Choose Picture");
            if (response == null || response.Length == 0 || response.Equals("Cancel"))
            {
                return;
            }
            MediaFile imgFile = null;
            if (response.Equals("Take Picture"))
            {
                imgFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    SaveToAlbum = true,
                    Directory = "FacerAttendance",
                    Name = "class"
                });
            }
            else if (response.Equals("Choose Picture"))
            {
                imgFile = await CrossMedia.Current.PickPhotoAsync();
            }
            // Delete temporary file we can use the album file
            System.IO.File.Delete(imgFile.Path);

            // Detect and identify people in the image
            identificationResults = await App.Reference.FaceAPI.Identify(imgFile.AlbumPath);

            // Show detection and identification results
            _parent.Refresh();
            await Navigation.PushModalAsync(this);
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
            await Navigation.PopModalAsync();
        }

    }
}