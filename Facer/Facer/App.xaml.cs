using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Facer.Data;
using Facer.Pages;
using Facer.FaceApi;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Facer
{
    public partial class App : Application
    {
        public static readonly string ApplicationName = "Facer";
        public static readonly string DataStorageFileName = ApplicationName + "_local_storage";
        private static App _reference;
        public static App Reference { get { return _reference; } }

        private DataStorage _data;
        private StudentDetector _faceAPI;
        private Printer _printer;
        public DataStorage Data { get { return _data; } }
        public StudentDetector FaceAPI { get { return _faceAPI; } }
        public Printer Printer { get { return _printer; } }

        public App()
        {
            _reference = this;
            _printer = new Printer();
            InitializeComponent();
            _data = new SQLiteDataStorage(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DataStorageFileName);
            _data.LoadData();
            MainPage = new MainPage();
        }

        protected async override void OnStart()
        {
            if(_faceAPI == null)
            {
                _faceAPI = await StudentDetector.CreateStudentDetector();
            }
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
