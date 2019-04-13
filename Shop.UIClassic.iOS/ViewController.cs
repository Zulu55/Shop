using Foundation;
using Newtonsoft.Json;
using Shop.Common.Helpers;
using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using UIKit;

namespace Shop.UIClassic.iOS
{
    public partial class ViewController : UIViewController
    {
        private ApiService apiService;

        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            this.EmailText.Text = "jzuluaga55@gmail.com";
            this.PasswordText.Text = "123456";
            this.PasswordText.SecureTextEntry = true;
            this.apiService = new ApiService();
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void LoginButton_TouchUpInside(UIButton sender)
        {
            if (string.IsNullOrEmpty(this.EmailText.Text))
            {
                var alert = UIAlertController.Create("Error", "You must enter an email.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordText.Text))
            {
                var alert = UIAlertController.Create("Error", "You must enter a password.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            this.DoLogin();
        }

        private async void DoLogin()
        {
            this.ActivityIndicator.StartAnimating();
            var request = new TokenRequest
            {
                Username = this.EmailText.Text,
                Password = this.PasswordText.Text
            };

            var response = await this.apiService.GetTokenAsync(
                "https://shopzulu.azurewebsites.net",
                "/Account",
                "/CreateToken",
                request);

            if (!response.IsSuccess)
            {
                this.ActivityIndicator.StopAnimating();
                var alert = UIAlertController.Create("Error", "User or password incorrect.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            var token = (TokenResponse)response.Result;

            var response2 = await this.apiService.GetListAsync<Product>(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Products",
                "bearer",
                token.Token);

            if (!response2.IsSuccess)
            {
                var alert = UIAlertController.Create("Error", response.Message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            var products = (List<Product>)response2.Result;

            Settings.UserEmail = this.EmailText.Text;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.Products = JsonConvert.SerializeObject(products);
            this.ActivityIndicator.StopAnimating();

            var board = UIStoryboard.FromName("Main", null);
            var productsViewController = board.InstantiateViewController("ProductsViewController");
            productsViewController.Title = "Products";
            this.NavigationController.PushViewController(productsViewController, true);

        }
    }
}