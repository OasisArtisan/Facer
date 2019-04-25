using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facer.Data;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddStudentPage : CustomContentPage
	{
        private StudentsPage _parent;
        private Dictionary<Image, string> _imageFiles;

        public AddStudentPage(StudentsPage parent) : this(parent, new Student())
        {
        }
        public AddStudentPage (StudentsPage page, Student student)
		{
            _parent = page;
            InitializeComponent ();
            if(student.Valid)
            {
                IDEntry.IsEnabled = false;
            }
            IDEntry.TextChanged += CheckIDField;
            CancelButton.Clicked += Cancel;
            SaveButton.Clicked += SaveStudent;

            _imageFiles = new Dictionary<Image, string>();
            foreach (View v in ImageArea.Children)
            {
                _imageFiles.Add((Image)v, null);
                var tap = (TapGestureRecognizer)v.GestureRecognizers.First();
                tap.Tapped += OnTakePicture;
            }
		}

        public void CheckIDField(object o, EventArgs e)
        {
            string s = Student.IsValidID(IDEntry.Text);
            if (IDEntry.Text.Length == 0 || s == null){
                IDEntryFeedback.IsVisible = false;
            } else
            {
                IDEntryFeedback.Text = s;
                IDEntryFeedback.IsVisible = true;
            }
        }

        public async void OnTakePicture(object o, EventArgs e)
        {
            Image img = (Image)o;
            string response = await DisplayActionSheet(null, "Cancel",null, "Take Picture", "Choose Picture");
            if (response == null || response.Length == 0 || response.Equals("Cancel"))
            {
                return;
            }
            // If this slot already has an image then delete its temporary file
            string imgFile = _imageFiles[img];
            if (imgFile != null)
            {
                System.IO.File.Delete(imgFile);
                _imageFiles[img] = null;
            }
            // Get image either by camera or from gallery
            MediaFile imgMediaFile = null;
            if (response.Equals("Take Picture"))
            {
                imgMediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                    SaveToAlbum = true,
                    Directory = "FacerTraining",
                    Name = "student"
                });
                if (imgMediaFile == null) return;
                App.Reference.Printer.PrintLine($"[AddStudentPage] image captured. Path: {imgMediaFile.Path}");
            } else if(response.Equals("Choose Picture"))
            {
                imgMediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (imgMediaFile == null) return;
                App.Reference.Printer.PrintLine($"[AddStudentPage] image chosen. Path: {imgMediaFile.Path}");
            }
            // Display acquired image
            img.Source = ImageSource.FromStream(() =>
            {
                return imgMediaFile.GetStream();
            });

            _imageFiles[img] = imgMediaFile.Path;
            // If we have acquired all images then we enable the save button
            bool completedImages = true;
            foreach (string i in _imageFiles.Values)
            {
                if(i == null)
                {
                    completedImages = false;
                    break;
                }
            }
            SaveButton.IsEnabled = completedImages;
        }

        public async void SaveStudent(object s, EventArgs e)
        {
            if (Student.IsValidID(IDEntry.Text) == null)
            {
                Student st = new Student()
                {
                    ID = IDEntry.Text,
                    FirstName = FirstNameEntry.Text,
                    LastName = LastNameEntry.Text
                };

                (App.Reference.MainPage as MainPage).SwitchToTab(2);
                await Navigation.PopModalAsync();
                await App.Reference.FaceAPI.AddStudentAsync(st, _imageFiles.Values.ToArray());
                App.Reference.Data.EnrollStudent(st);
                // Delete temporary files
                foreach (string path in _imageFiles.Values)
                {
                    System.IO.File.Delete(path);
                }
                _parent.Refresh();
                (App.Reference.MainPage as MainPage).SwitchToTab(1);
            } else
            {
                await DisplayAlert("Invalid Student", "Cannot add student, ID must be valid!", "Ok");
            }
        }
        public async void Cancel(object s, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}