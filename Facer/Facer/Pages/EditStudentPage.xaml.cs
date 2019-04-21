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
	public partial class EditStudentPage : ContentPage
	{
        private StudentsPage _parent;
        private Dictionary<Image, MediaFile> _imageFiles;

        public EditStudentPage(StudentsPage parent) : this(parent, new Student())
        {
        }
        public EditStudentPage (StudentsPage parent, Student student)
		{
            _parent = parent;
            
            InitializeComponent ();
            if(student.Valid)
            {
                IDEntry.IsEnabled = false;
            }
            IDEntry.TextChanged += CheckIDField;
            CancelButton.Clicked += Cancel;
            SaveButton.Clicked += SaveStudent;

            _imageFiles = new Dictionary<Image, MediaFile>();
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
            MediaFile imgFile = _imageFiles[img];
            if (imgFile != null)
            {
                System.IO.File.Delete(imgFile.Path);
                _imageFiles[img] = null;
            }
            // Get image either by camera or from gallery
            if (response.Equals("Take Picture"))
            {
                imgFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                    SaveToAlbum = true,
                    Directory = "FacerTraining",
                    Name = "student"
                });
            } else if(response.Equals("Choose Picture"))
            {
                imgFile = await CrossMedia.Current.PickPhotoAsync();
            }
            // Display acquired image
            img.Source = ImageSource.FromStream(() =>
            {
                return imgFile.GetStream();
            });

            _imageFiles[img] = imgFile;
            // If we have acquired all images then we enable the save button
            bool completedImages = true;
            foreach (MediaFile mf in _imageFiles.Values)
            {
                if(mf == null)
                {
                    completedImages = false;
                    break;
                }
            }
            SaveButton.IsEnabled = completedImages;
        }

        public void SaveStudent(object s, EventArgs e)
        {
            if (Student.IsValidID(IDEntry.Text) == null)
            {
                Student st = new Student()
                {
                    ID = int.Parse(IDEntry.Text),
                    FirstName = FirstNameEntry.Text,
                    LastName = LastNameEntry.Text
                };
                App.Reference.Data.EnrollStudent(st);
                TrainOnStudent(st, _imageFiles.Values);
                Navigation.PopModalAsync();
                _parent.UpdateListView();
            } else
            {
                DisplayAlert("Invalid Student", "Cannot add student, ID must be valid!", "Ok");
            }
        }
        public void Cancel(object s, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        // [TODO:Abdullah]
        public void TrainOnStudent(Student st, IEnumerable<MediaFile> images)
        {
            // Training logic

            //Dispose of temporary images
            foreach (var i in images)
            {
                System.IO.File.Delete(i.Path);
            }
        }
    }
}