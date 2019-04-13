namespace Shop.UIClassic.iOS
{
    using System;
    using System.Collections.Generic;
    using Common.Helpers;
    using Common.Models;
    using DataSources;
    using Newtonsoft.Json;
    using UIKit;

    public partial class ProductsViewController : UIViewController
    {
        public ProductsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var products = JsonConvert.DeserializeObject<List<Product>>(Settings.Products);
            var datasource = new ProductsDataSource(products);
            this.TableView.Source = datasource;
        }
    }
}