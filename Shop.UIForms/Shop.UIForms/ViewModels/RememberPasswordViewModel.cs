namespace Shop.UIForms.ViewModels
{
    using System.Windows.Input;
    using Common.Helpers;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Shop.Common.Models;
    using Xamarin.Forms;

    public class RememberPasswordViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private readonly ApiService apiService;

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public string Email { get; set; }

        public ICommand RecoverCommand => new RelayCommand(this.Recover);

        public RememberPasswordViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void Recover()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email.",
                    "Accept");
                return;
            }

            if (!RegexHelper.IsValidEmail(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a valid email.",
                    "Accept");
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new RecoverPasswordRequest
            {
                Email = this.Email
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.RecoverPasswordAsync(
                url,
                "/api",
                "/Account/RecoverPassword",
                request);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                "Accept");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
