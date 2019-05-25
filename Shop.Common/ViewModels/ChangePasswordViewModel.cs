namespace Shop.Common.ViewModels
{
    using System.Windows.Input;
    using Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;

    public class ChangePasswordViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private MvxCommand changePasswordCommand;
        private string currentPassword;
        private string newPassword;
        private string confirmPassword;
        private bool isLoading;

        public ChangePasswordViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.IsLoading = false;
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                this.changePasswordCommand = this.changePasswordCommand ?? new MvxCommand(this.ChangePassword);
                return this.changePasswordCommand;
            }
        }

        public string CurrentPassword
        {
            get => this.currentPassword;
            set => this.SetProperty(ref this.currentPassword, value);
        }

        public string NewPassword
        {
            get => this.newPassword;
            set => this.SetProperty(ref this.newPassword, value);
        }

        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set => this.SetProperty(ref this.confirmPassword, value);
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(this.CurrentPassword))
            {
                this.dialogService.Alert("Error", "You must enter a current pasword.", "Accept");
                return;
            }

            if (!this.CurrentPassword.Equals(Settings.UserPassword))
            {
                this.dialogService.Alert("Error", "The current pasword is not correct.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                this.dialogService.Alert("Error", "You must enter a new pasword.", "Accept");
                return;
            }

            if (this.NewPassword.Length < 6)
            {
                this.dialogService.Alert("Error", "The new password must be a least 6 characters.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.ConfirmPassword))
            {
                this.dialogService.Alert("Error", "You must enter a pasword confirm.", "Accept");
                return;
            }

            if (!this.NewPassword.Equals(this.ConfirmPassword))
            {
                this.dialogService.Alert("Error", "The pasword and confirm does not math.", "Accept");
                return;
            }

            this.IsLoading = true;

            var request = new ChangePasswordRequest
            {
                Email = Settings.UserEmail,
                NewPassword = this.NewPassword,
                OldPassword = this.CurrentPassword
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.ChangePasswordAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Account/ChangePassword",
                request,
                "bearer",
                token.Token);

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            Settings.UserPassword = this.NewPassword;
            await this.navigationService.Close(this);
        }
    }
}