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
    public partial class AdminPage : CustomContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
            ClearConsoleButton.Clicked += (e, o) =>
            {
                ConsoleOutputView.Clear();
            };
            ClearDataButton.Clicked += async (e, o) =>
            {
                bool confirmed = await DisplayAlert("Warning", "This will clear all the information in the local database and server", "Clear", "Cancel");
                if (confirmed)
                {
                    App.Reference.Data.ClearData();
                }
            };
            App.Reference.Printer.Subscribe(ConsoleOutputView);
        }
    }
}