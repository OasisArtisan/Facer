using Facer.Data;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AttendanceRecordsPage : ContentPage
	{
		public AttendanceRecordsPage ()
		{
			InitializeComponent ();
            AddRecordButton.Clicked += AddRecord;
            AttendanceRecordsListView.ItemsSource = App.Reference.Data.AttendanceRecordsEnumerable();
            AttendanceRecordsListView.Refreshing += (s, e) =>
             {
                 // Very bad way to actually update the list view.
                 // It is better to use or make a data structure that implements the INotifyCollectionChanged like an ObservableCollection
                 // That way changes are automatically reflected in a safe and efficient way
                 AttendanceRecordsListView.ItemsSource = null;
                 AttendanceRecordsListView.ItemsSource = App.Reference.Data.AttendanceRecordsEnumerable();
                 AttendanceRecordsListView.EndRefresh();
             };
        }

        public async void AddRecord(object o, EventArgs e)
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
            AttendanceRecord ar = new AttendanceRecord()
            {
                Date = DateTime.Now
            };
            foreach(Student st in GetAttendanceFromPicture(imgFile))
            {
                ar.AddStudent(st);
            }
            App.Reference.Data.CreateAttendanceRecord(ar);
            AttendanceRecordsListView.BeginRefresh();
        }

        // [TODO:ABDULLAH]
        public List<Student> GetAttendanceFromPicture(MediaFile mf)
        {
            System.IO.File.Delete(mf.Path);
            return new List<Student>();
        }
	}
}