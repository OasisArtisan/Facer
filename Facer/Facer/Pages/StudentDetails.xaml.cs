using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Facer.Data;

namespace Facer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentDetails : CustomContentPage
    {
        private StudentsPage _parent;
        private Student _student;
        public StudentDetails(StudentsPage parent, Student st)
        {
            _student = st;
            _parent = parent;
            InitializeComponent();
            NameLabel.Text = st.Formatted;
            int totalRecords = 0;
            int attendedRecords = 0;
            foreach (AttendanceRecord ar in App.Reference.Data.AttendanceRecordsEnumerable())
            {
                totalRecords += 1;
                if (ar.ContainsStudent(st))
                {
                    attendedRecords += 1;
                }
            }
            if(totalRecords == 0)
            {
                PercentageLabel.Text = $"{attendedRecords}/{totalRecords} Attended";
            } else
            {
                PercentageLabel.Text = $"{attendedRecords}/{totalRecords}   {attendedRecords*100f / totalRecords}% Attended";
            }
            DeleteButton.Clicked += (e, o) =>
            {
                App.Reference.Data.RemoveStudent(st);
                _parent.Refresh();
                Navigation.PopModalAsync();
            };
            CancelButton.Clicked += (e, o) =>
            {
                Navigation.PopModalAsync();
            };
        }
    }
}