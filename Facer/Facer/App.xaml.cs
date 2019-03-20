using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Facer.Data;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Facer
{
    public partial class App : Application
    {
        public static readonly string ApplicationName = "Facer";
        public static readonly string DataStorageFileName = ApplicationName + "_local_storage";

        public DataStorage AppDataStorage { get; }

        public App()
        {
            InitializeComponent();
            AppDataStorage = new SQLiteDataStorage(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataStorageFileName);
            Student.DataStorage = AppDataStorage;
            AttendanceRecord.DataStorage = AppDataStorage;
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
