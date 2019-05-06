namespace FourWays.Classic.Droid
{
    using System;
    using Android.App;
    using Android.OS;
    using Android.Support.V7.App;
    using Android.Widget;
    using Core.Services;

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText amountEditText;
        private SeekBar generositySeekBar;
        private TextView tipTextView;
        private ICalculationService calculationService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_main);
            this.calculationService = new CalculationService();
            this.FindViews();
            this.SetupEvents();
            this.RefreshTip();
        }

        private void SetupEvents()
        {
            this.amountEditText.TextChanged += AmountEditText_TextChanged;
            this.generositySeekBar.ProgressChanged += GenerositySeekBar_ProgressChanged;
        }

        private void GenerositySeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            this.RefreshTip();
        }

        private void AmountEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            this.RefreshTip();
        }

        private void RefreshTip()
        {
            var amount = Convert.ToDecimal(this.amountEditText.Text);
            var generosity = (double)this.generositySeekBar.Progress;
            this.tipTextView.Text = $"{this.calculationService.TipAmount(amount, generosity):C2}";
        }

        private void FindViews()
        {
            this.amountEditText = this.FindViewById<EditText>(Resource.Id.amountEditText);
            this.generositySeekBar = this.FindViewById<SeekBar>(Resource.Id.generositySeekBar);
            this.tipTextView = this.FindViewById<TextView>(Resource.Id.tipTextView);
        }
    }
}