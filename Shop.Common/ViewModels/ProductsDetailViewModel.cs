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

    public class ProductsDetailViewModel : MvxViewModel<NavigationArgs>
    {
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private Product product;
        private bool isLoading;
        private MvxCommand updateCommand;
        private MvxCommand deleteCommand;

        public ProductsDetailViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.IsLoading = false;
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public Product Product
        {
            get => this.product;
            set => this.SetProperty(ref this.product, value);
        }

        public ICommand UpdateCommand
        {
            get
            {
                this.updateCommand = this.updateCommand ?? new MvxCommand(this.Update);
                return this.updateCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                this.deleteCommand = this.deleteCommand ?? new MvxCommand(this.Delete);
                return this.deleteCommand;
            }
        }

        private async void Delete()
        {
            //TODO: Ask for confirmation
            this.IsLoading = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await this.apiService.DeleteAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Products",
                product.Id,
                "bearer",
                token.Token);

            this.IsLoading = false;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            await this.navigationService.Close(this);
        }

        private async void Update()
        {
            if (string.IsNullOrEmpty(this.Product.Name))
            {
                this.dialogService.Alert("Error", "You must enter a product name.", "Accept");
                return;
            }

            if (this.Product.Price <= 0)
            {
                this.dialogService.Alert("Error", "The price must be a number greather than zero.", "Accept");
                return;
            }

            this.IsLoading = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await this.apiService.PutAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Products",
                product.Id,
                product,
                "bearer",
                token.Token);

            this.IsLoading = false;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            await this.navigationService.Close(this);
        }

        public override void Prepare(NavigationArgs parameter)
        {
            this.product = parameter.Product;
        }
    }
}
