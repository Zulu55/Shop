namespace Shop.Common.ViewModels
{
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Services;
    using Shop.Common.Helpers;
    using Newtonsoft.Json;

    public class LoginViewModel : MvxViewModel
    {
        private string email;
        private string password;
        private MvxCommand loginCommand;
        private MvxCommand registerCommand;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly INetworkProvider networkProvider;
        private bool isLoading;

        public LoginViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService,
            INetworkProvider networkProvider)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.networkProvider = networkProvider;

            this.Email = "jzuluaga55@gmail.com";
            this.Password = "123456";
            this.IsLoading = false;
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public ICommand LoginCommand
        {
            get
            {
                this.loginCommand = this.loginCommand ?? new MvxCommand(this.DoLoginCommand);
                return this.loginCommand;
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                this.registerCommand = this.registerCommand ?? new MvxCommand(this.DoRegisterCommand);
                return this.registerCommand;
            }
        }

        private async void DoRegisterCommand()
        {
            await this.navigationService.Navigate<RegisterViewModel>();
        }

        private async void DoLoginCommand()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                this.dialogService.Alert("Error", "You must enter an email.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                this.dialogService.Alert("Error", "You must enter a password.", "Accept");
                return;
            }

            if (!this.networkProvider.IsConnectedToWifi())
            {
                this.dialogService.Alert("Error", "The App requiered a internet connection, please check and try again.", "Accept");
                return;
            }

            this.IsLoading = true;

            var request = new TokenRequest
            {
                Password = this.Password,
                Username = this.Email
            };

            var response = await this.apiService.GetTokenAsync(
                "https://shopzulu.azurewebsites.net",
                "/Account",
                "/CreateToken",
                request);

            if (!response.IsSuccess)
            {
                this.IsLoading = false;
                this.dialogService.Alert("Error", "User or password incorrect.", "Accept");
                return;
            }

            var token = (TokenResponse)response.Result;
            Settings.UserEmail = this.Email;
            Settings.Token = JsonConvert.SerializeObject(token);
            this.IsLoading = false;
            await this.navigationService.Navigate<ProductsViewModel>();
        }
    }
}