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
    public partial class ChooseStudentPage : CustomContentPage
    {
        private CustomContentPage _parent;
        private AttendanceRecord _record;
        public ChooseStudentPage(CustomContentPage parent, AttendanceRecord ar)
        {
            _parent = parent;
            _record = ar;
            InitializeComponent();
            List<Student> list = new List<Student>();
            foreach(Student st in App.Reference.Data.EnrolledStudentsEnumerable())
            {
                if (!ar.ContainsStudent(st))
                {
                    list.Add(st);
                }
            }
            StudentListView.ItemsSource = list;
            StudentListView.ItemTapped += async (e, o) =>
            {
                Student toEnroll = o.Item as Student;
                ar.AddStudent(toEnroll);
                _parent.Refresh();
                await Navigation.PopModalAsync();
            };
        }
        public override void Refresh()
        {
            StudentListView.BeginRefresh();
        }
    }
}