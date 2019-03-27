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
	public partial class EditStudentPage : ContentPage
	{
        public EditStudentPage() : this(new Student())
        {
        }
        public EditStudentPage (Student student)
		{
			InitializeComponent ();
            if(student.Valid)
            {
                IDEntry.IsEnabled = false;
            }
            IDEntry.TextChanged += CheckIDField;
            CancelButton.Clicked += Cancel;
            SaveButton.Clicked += SaveStudent;
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
                Navigation.PopModalAsync();
            } else
            {
                DisplayAlert("Invalid Student", "Cannot add student, ID must be valid!", "Ok");
            }
            
        }
        public void Cancel(object s, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}