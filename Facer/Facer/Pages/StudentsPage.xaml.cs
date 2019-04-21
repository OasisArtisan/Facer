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
            StudentListView.ItemsSource = App.Reference.Data.EnrolledStudentsEnumerable();
            StudentListView.Refreshing += (s, e) =>
            {
                // Very bad way to actually update the list view.
                // It is better to use or make a data structure that implements the INotifyCollectionChanged like an ObservableCollection
                // That way changes are automatically reflected in a safe and efficient way
                StudentListView.ItemsSource = null;
                StudentListView.ItemsSource = App.Reference.Data.EnrolledStudentsEnumerable();
                StudentListView.EndRefresh();
            };
        }

        public void EditStudent(object o, EventArgs e)
        {
            Navigation.PushModalAsync(new EditStudentPage(this));
        }

        public void UpdateListView()
        {
            StudentListView.BeginRefresh();
        }
	}
}