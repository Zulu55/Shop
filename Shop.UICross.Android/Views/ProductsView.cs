using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using Shop.Common.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Shop.UICross.Android.Views
{
    [Activity(
        Label = "@string/products",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductsView : MvxAppCompatActivity<ProductsViewModel>
    {
        private readonly string[] menuOptions = { "Add Product", "Edit User", "Change Password", "Maps", "Close Session" };
        private ListView drawerListView;
        private DrawerLayout drawer;
        private ActionBarDrawerToggle toggle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.ProductsPage);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.menu_icon);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            drawerListView = FindViewById<ListView>(Resource.Id.drawerListView);
            drawerListView.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleListItem1, menuOptions);
            drawerListView.ItemClick += listView_ItemClick;
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawerLayout);
            toggle = new ActionBarDrawerToggle(
                this,
                drawer,
                toolbar,
                Resource.String.navigation_drawer_open,
                Resource.String.navigation_drawer_closed);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
        }

        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    StartActivity(typeof(AddProductView));
                    break;
                case 1:
                    StartActivity(typeof(EditUserView));
                    break;
                case 2:
                    StartActivity(typeof(ChangePasswordView));
                    break;
                case 3:
                    StartActivity(typeof(MapsView));
                    break;
                case 4:
                    OnBackPressed();
                    break;
            }

            drawer.CloseDrawer(drawerListView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (toggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}