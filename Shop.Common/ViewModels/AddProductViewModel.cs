namespace Shop.Common.ViewModels
{
    using Helpers;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;
    using System.Windows.Input;

    public class AddProductViewModel : MvxViewModel
    {
        private string name;
        private string price;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;
        private readonly IPictureService pictureService;
        private bool isLoading;
        private byte[] theRawImageBytes;
        private MvxCommand addProductCommand;
        private MvxCommand selectPictureCommand;

        public AddProductViewModel(
            IApiService apiService,
            IDialogService dialogService,
            IMvxNavigationService navigationService,
            IPictureService pictureService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.pictureService = pictureService;
        }

        public byte[] TheRawImageBytes
        {
            get => this.theRawImageBytes;
            set
            {
                this.theRawImageBytes = value;
                RaisePropertyChanged(() => TheRawImageBytes);
            }
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public string Price
        {
            get => this.price;
            set => this.SetProperty(ref this.price, value);
        }

        public ICommand AddProductCommand
        {
            get
            {
                this.addProductCommand = this.addProductCommand ?? new MvxCommand(this.AddProduct);
                return this.addProductCommand;
            }
        }

        public ICommand SelectPictureCommand
        {
            get
            {
                this.selectPictureCommand = this.selectPictureCommand ?? new MvxCommand(this.AddPictureFromCameraOrGallery);
                return this.selectPictureCommand;
            }
        }

        private void AddPictureFromCameraOrGallery()
        {
            this.dialogService.Confirm(
                "Confirm",
                "Select your photo from",
                "Camera",
                "Gallery",
                () => { this.pictureService.TakeNewPhoto(ProcessPhoto, null); },
                () => { this.pictureService.SelectExistingPicture(ProcessPhoto, null); });
        }

        private void ProcessPhoto(byte[] image)
        {
            this.TheRawImageBytes = image;
        }

        private async void AddProduct()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                this.dialogService.Alert("Error", "You must enter a product name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                this.dialogService.Alert("Error", "You must enter a product price.", "Accept");
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                this.dialogService.Alert("Error", "The price must be a number greather than zero.", "Accept");
                return;
            }

            this.IsLoading = true;

            var product = new Product
            {
                IsAvailabe = true,
                Name = this.Name,
                Price = price,
                User = new User { UserName = Settings.UserEmail },
                ImageArray = this.TheRawImageBytes
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await this.apiService.PostAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Products",
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
    }
}