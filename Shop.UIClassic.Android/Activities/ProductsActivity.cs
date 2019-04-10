using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Shop.Common.Models;
using Shop.Common.Services;
using Shop.UIClassic.Android.Adapters;
using Shop.UIClassic.Android.Helpers;

namespace Shop.UIClassic.Android.Activities
{
    [Activity(
        Label = "@string/products",
        Theme = "@style/AppTheme")]
    public class ProductsActivity : AppCompatActivity
    {
        private TokenResponse token;
        private string email;
        private ApiService apiService;
        private ListView productsListView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.ProductsPage);

            this.productsListView = this.FindViewById<ListView>(Resource.Id.productsListView);

            this.email = Intent.Extras.GetString("email");
            var tokenString = Intent.Extras.GetString("token");
            this.token = JsonConvert.DeserializeObject<TokenResponse>(tokenString);

            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetListAsync<Product>(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Products",
                "bearer",
                this.token.Token);

            if (!response.IsSuccess)
            {
                DiaglogService.ShowMessage(this, "Error", response.Message, "Accept");
                return;
            }

            var products = (List<Product>)response.Result;
            this.productsListView.Adapter = new ProductsListAdapter(this, products);
            this.productsListView.FastScrollEnabled = true;
        }
    }
}