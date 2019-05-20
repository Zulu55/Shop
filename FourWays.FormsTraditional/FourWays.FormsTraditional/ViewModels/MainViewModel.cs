namespace FourWays.FormsTraditional.ViewModels
{
    using FourWays.Core.Services;

    public class MainViewModel : BaseViewModel
    {
        private ICalculationService calculationService;
        private decimal amount;
        private double generosity;
        private decimal tip;

        public decimal Amount
        {
            get { return this.amount; }
            set
            {
                this.SetValue(ref this.amount, value);
                this.Recalculate();
            }
        }

        public double Generosity
        {
            get { return this.generosity; }
            set
            {
                this.SetValue(ref this.generosity, value);
                this.Recalculate();
            }
        }

        public decimal Tip
        {
            get { return this.tip; }
            set
            {
                this.SetValue(ref this.tip, value);
            }
        }

        public MainViewModel()
        {
            this.calculationService = new CalculationService();
            this.Amount = 100;
            this.Generosity = 10;
        }

        private void Recalculate()
        {
            this.Tip = this.calculationService.TipAmount(this.Amount, this.Generosity);
        }
    }
}
