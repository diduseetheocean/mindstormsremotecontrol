using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using RemoteControl.CrossDependency;
using RemoteControl.Droid.CrossDependency;

[assembly: Xamarin.Forms.Dependency(typeof(Loading))]
namespace RemoteControl.Droid.CrossDependency
{
    public class Loading : ILoading
    {
        private ViewGroup decorView;
        ViewGroup layoutResource;
        FrameLayout loadingFrame;

        public void Hide()
        {
            decorView.RemoveView(layoutResource);
        }

        public void Show()
        {
            decorView = (ViewGroup)((Activity)Xamarin.Forms.Forms.Context).Window.DecorView;

            var inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            layoutResource = (ViewGroup)inflater.Inflate(Resource.Layout.LoadingLayout, null);

            var progressBar = new ProgressBar(Xamarin.Forms.Forms.Context, null, Android.Resource.Attribute.ProgressBarStyleLarge);
            var scale = (int)(Xamarin.Forms.Forms.Context.Resources.DisplayMetrics.Density * 72);
            progressBar.LayoutParameters = new LinearLayout.LayoutParams(scale, scale);
            progressBar.Visibility = ViewStates.Visible;

            layoutResource.AddView(progressBar);

            decorView.AddView(layoutResource);
        }
    }
}
