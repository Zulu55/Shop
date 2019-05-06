namespace FourWays.Cross.Droid.Views
{
    using Android.App;
    using Android.OS;
    using Core.ViewModels;
    using MvvmCross.Platforms.Android.Views;

    [Activity(Label = "@string/app_name")]
    public class TipView : MvxActivity<TipViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.TipPage);
        }
    }
}