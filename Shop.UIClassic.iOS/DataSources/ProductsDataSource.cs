namespace Shop.UIClassic.iOS.DataSources
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Foundation;
    using Shop.UIClassic.iOS.Cells;
    using UIKit;

    public class ProductsDataSource : UITableViewSource
    {
        private readonly List<Product> products;
        private readonly NSString cellIdentifier = new NSString("ProductCell");

        public ProductsDataSource(List<Product> products)
        {
            this.products = products;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifier) as ProductCell;

            if (cell == null)
            {
                cell = new ProductCell(cellIdentifier);
            }

            var product = products[indexPath.Row];
            cell.UpdateCell(product.Name, $"{product.Price:C2}", UIImage.FromFile(product.ImageUrl));

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.products.Count;
        }
    }
}