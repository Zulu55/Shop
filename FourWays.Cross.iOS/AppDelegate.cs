namespace FourWays.Cross.iOS
{
    using Core;
    using Foundation;
    using MvvmCross.Platforms.Ios.Core;

    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<MvxIosSetup<App>, App>
    {
    }
}