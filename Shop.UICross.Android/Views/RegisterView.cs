using Android.Views;
using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using Shop.Common.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Shop.UICross.Android.Views
{
    [Activity(Label = "@string/register")]
    public class RegisterView : MvxAppCompatActivity<RegisterViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.RegisterPage);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var actionBar = SupportActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == global::Android.Resource.Id.Home) { OnBackPressed(); }
            return base.OnOptionsItemSelected(item);
        }
    }
}