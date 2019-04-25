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
using Facer.FaceApi;

namespace Facer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AttendanceRecordsPage : CustomContentPage
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
            AttendanceRecordsListView.ItemTapped += (s, e) =>
            {
                Navigation.PushModalAsync(new AttendanceRecordDetails(e.Item as AttendanceRecord));
            };
        }

        public void AddRecord(object o, EventArgs e)
        {
            IdentificationPage ip = new IdentificationPage(this);
            ip.TakeRecordImage(o,e);
        }

        public override void Refresh()
        {
            AttendanceRecordsListView.BeginRefresh();
        }
    }
}