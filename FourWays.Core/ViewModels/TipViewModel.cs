namespace FourWays.Core.ViewModels
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Services;

    public class TipViewModel : MvxViewModel
    {
        #region Attributes
        private readonly ICalculationService calculationService;
        private decimal subTotal;
        private int generosity;
        private decimal tip;
        #endregion

        #region Properties
        public decimal SubTotal
        {
            get
            {
                return this.subTotal;
            }
            set
            {
                this.subTotal = value;
                this.RaisePropertyChanged(() => this.SubTotal);
                this.Recalculate();
            }
        }

        public decimal Tip
        {
            get
            {
                return this.tip;
            }
            set
            {
                this.tip = value;
                this.RaisePropertyChanged(() => this.Tip);
            }
        }

        public int Generosity
        {
            get
            {
                return this.generosity;
            }
            set
            {
                this.generosity = value;
                this.RaisePropertyChanged(() => this.Generosity);
                this.Recalculate();
            }
        }
        #endregion

        #region Constructors
        public TipViewModel(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
        }
        #endregion

        #region Methods
        public override async Task Initialize()
        {
            await base.Initialize();

            this.SubTotal = 100;
            this.Generosity = 10;
            this.Recalculate();
        }

        private void Recalculate()
        {
            this.Tip = this.calculationService.TipAmount(this.SubTotal, this.Generosity);
        }
        #endregion
    }
}
