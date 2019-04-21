using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Facer.Data;
using Facer.Pages;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Facer
{
    public partial class App : Application
    {
        public static readonly string ApplicationName = "Facer";
        public static readonly string DataStorageFileName = ApplicationName + "_local_storage";
        private static App _reference;
        public static App Reference { get { return _reference; } }

        public DataStorage Data { get; }
        public App()
        {
            _reference = this;
            InitializeComponent();
            Data = new SQLiteDataStorage(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataStorageFileName);
            Data.LoadData();
            
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
