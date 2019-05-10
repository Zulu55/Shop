namespace Shop.UICross.Android.Views
{
    using global::Android.App;
    using global::Android.Content.PM;
    using MvvmCross.Platforms.Android.Views;

    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashView : MvxSplashScreenActivity
    {
        public SplashView() : base(Resource.Layout.SplashPage)
        {
        }
    }
}