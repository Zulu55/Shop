namespace FourWays.Classic.iOS
{
    using System;
    using FourWays.Core.Services;
    using UIKit;

    public partial class ViewController : UIViewController
    {
        private readonly ICalculationService calculationService;

        public ViewController(IntPtr handle) : base(handle)
        {
            this.calculationService = new CalculationService();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.AmountText.EditingChanged += AmountText_EditingChanged;
            this.GenerositySlider.ValueChanged += GenerositySlider_ValueChanged;
        }

        private void GenerositySlider_ValueChanged(object sender, EventArgs e)
        {
            this.RefreshTip();
        }

        private void RefreshTip()
        {
            var amount = Convert.ToDecimal(this.AmountText.Text);
            var generosity = (double)this.GenerositySlider.Value;
            this.TipLabel.Text = $"{this.calculationService.TipAmount(amount, generosity):C2}";
        }

        private void AmountText_EditingChanged(object sender, EventArgs e)
        {
            this.RefreshTip();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}