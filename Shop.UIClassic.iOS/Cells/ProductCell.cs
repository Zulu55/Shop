namespace Shop.UIClassic.iOS.Cells
{
    using System.Drawing;
    using Foundation;
    using UIKit;

    public class ProductCell : UITableViewCell
    {
        private readonly UILabel nameLabel;
        private readonly UILabel priceLabel;
        private readonly UIImageView imageView;

        public ProductCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.Gray;

            this.imageView = new UIImageView();
            this.nameLabel = new UILabel();
            this.priceLabel = new UILabel()
            {
                TextAlignment = UITextAlignment.Right
            };

            this.ContentView.Add(this.nameLabel);
            this.ContentView.Add(this.priceLabel);
            this.ContentView.Add(this.imageView);
        }

        public void UpdateCell(string caption, string subtitle, UIImage image)
        {
            this.imageView.Image = image;
            this.nameLabel.Text = caption;
            this.priceLabel.Text = subtitle;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.imageView.Frame = new RectangleF((float)this.ContentView.Bounds.Width - 63, 5, 33, 33);
            this.nameLabel.Frame = new RectangleF(5, 4, (float)this.ContentView.Bounds.Width - 63, 25);
            this.priceLabel.Frame = new RectangleF(200, 10, 100, 20);
        }
    }
}