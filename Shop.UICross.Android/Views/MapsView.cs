using Android.App;
using Android.Gms.Maps;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using System;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Shop.UICross.Android.Views
{
    [Activity(Label = "@string/maps")]
    public class MapsView : MvxAppCompatActivity, IOnMapReadyCallback
    {
        private GoogleMap googlemap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.MapsPage);
            SetUpMap();
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var actionBar = SupportActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
            }

        }

        [Obsolete]
        private void SetUpMap()
        {
            if (googlemap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            googlemap = googleMap;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == global::Android.Resource.Id.Home)
            {
                OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}