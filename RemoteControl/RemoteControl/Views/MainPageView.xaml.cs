using RemoteControl.ViewModels;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace RemoteControl
{
    public partial class MainPageView : TabbedPage
    {
        public MainPageView()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }
    }
}