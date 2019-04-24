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
        private AttendanceRecord record;
		public AttendanceRecordDetails (AttendanceRecord ar)
		{
			InitializeComponent ();
            record = ar;
            DateLabel.Text = ar.Formatted;
            StudentsList.ItemsSource = record.EnumerateAttendedStudents();
            StudentsList.Refreshing += (s, e) =>
            {
                StudentsList.ItemsSource = null;
                StudentsList.ItemsSource = record.EnumerateAttendedStudents();
                StudentsList.EndRefresh();
            };
        }

        public override void Refresh()
        {
            StudentsList.BeginRefresh();
        }
    }
}
