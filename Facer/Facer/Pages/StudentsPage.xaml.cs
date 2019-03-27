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
	public partial class StudentsPage : ContentPage
	{
		public StudentsPage ()
		{
			InitializeComponent ();
            AddStudentButton.Clicked += EditStudent;
            //StudentListView.ItemsSource = App.Reference.Data.EnumerateEnrolledStudents()
        }

        public void EditStudent(object o, EventArgs e)
        {
            Navigation.PushModalAsync(new EditStudentPage());
        }
	}
}