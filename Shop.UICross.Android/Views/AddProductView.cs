using Android.Views;
using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using Shop.Common.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Shop.UICross.Android.Views
{

    [Activity(Label = "@string/add_product")]
    public class AddProductView : MvxAppCompatActivity<AddProductViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.AddProductPage);

            if (global::Android.Support.V4.App.ActivityCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.Camera) != global::Android.Content.PM.Permission.Granted ||
                global::Android.Support.V4.App.ActivityCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.WriteExternalStorage) != global::Android.Content.PM.Permission.Granted ||
                global::Android.Support.V4.App.ActivityCompat.CheckSelfPermission(this, global::Android.Manifest.Permission.ReadExternalStorage) != global::Android.Content.PM.Permission.Granted)
            {
                global::Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { global::Android.Manifest.Permission.Camera, global::Android.Manifest.Permission.WriteExternalStorage, global::Android.Manifest.Permission.ReadExternalStorage }, 1);
            }

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