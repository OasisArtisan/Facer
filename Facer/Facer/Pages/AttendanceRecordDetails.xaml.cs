using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facer.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Facer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AttendanceRecordDetails : CustomContentPage
	{
        private AttendanceRecord _record;
		public AttendanceRecordDetails (AttendanceRecord ar)
		{
			InitializeComponent ();
            _record = ar;
            DateLabel.Text = ar.Formatted;
            int arCount = ar.AttendedStudentsCount;
            int totalCount = App.Reference.Data.EnrolledStudentsCount;
            if(totalCount == 0)
            {
                AttendedLabel.Text = $"{arCount}/{totalCount} Attended";
            } else
            {
                AttendedLabel.Text = $"{arCount}/{totalCount}   {arCount * 100f / totalCount}% Attended";
            }
            StudentsList.ItemsSource = _record.EnumerateAttendedStudents();
            StudentsList.Refreshing += (s, e) =>
            {
                StudentsList.ItemsSource = null;
                StudentsList.ItemsSource = _record.EnumerateAttendedStudents();
                StudentsList.EndRefresh();
            };
            AddStudentButton.Clicked += async (e, o) =>
            {
                await Navigation.PushModalAsync(new ChooseStudentPage(this, _record));
            };
            StudentsList.ItemTapped += async (e, o) =>
            {
                Student s = o.Item as Student;
                bool confirmed = await DisplayAlert(null, $"Do you want to remove {s.Formatted} from this record?", "Yes", "No");
                if (confirmed)
                {
                    _record.RemoveStudent(s);
                    Refresh();
                }
            };
        }

        public override void Refresh()
        {
            StudentsList.BeginRefresh();
        }
    }
}
