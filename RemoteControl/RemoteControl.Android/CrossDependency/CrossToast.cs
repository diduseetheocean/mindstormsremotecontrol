using Android.Widget;
using RemoteControl.CrossDependency;
using RemoteControl.Droid.CrossDependency;

[assembly: Xamarin.Forms.Dependency(typeof(CrossToast))]
namespace RemoteControl.Droid.CrossDependency
{
    public class CrossToast : ICrossToast
    {
        public void ShowToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}
