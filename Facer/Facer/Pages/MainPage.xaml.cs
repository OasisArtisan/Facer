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
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            InitializeComponent();
        }
        public void SwitchToTab(int i)
        {
            this.CurrentPage = this.Children[i];
        }
        public void RefreshTabs()
        {
            foreach (ContentPage cp in this.Children){
                (cp as CustomContentPage).Refresh();
            }
        }
    }
}