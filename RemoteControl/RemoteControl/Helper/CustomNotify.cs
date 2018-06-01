using System;
using RemoteControl.CrossDependency;
using Xamarin.Forms;

namespace RemoteControl.Helper
{
    public static class CustomNotify
    {
        public static void ShowLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<ILoading>().Show();
            });
        }

        public static void HideLoading()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<ILoading>().Hide();
            });
        }

        public static void ShowToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DependencyService.Get<ICrossToast>().ShowToast(message);
            });
        }
    }
}
