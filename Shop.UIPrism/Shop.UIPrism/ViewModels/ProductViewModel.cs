using Prism.Commands;
using Prism.Navigation;
using Shop.Common.Models;
using Shop.Common.Services;
using System;
using Xamarin.Forms;

namespace Shop.UIPrism.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Product _product;
        private bool _isEdit;
        private bool _isEnabled;
        private bool _isRunning;
        private ImageSource _imageSource;
        private DelegateCommand _saveCommand;
        private DelegateCommand _deleteCommand;
        private DelegateCommand _changeImageCommand;

        public ProductViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(Save));

        public DelegateCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(Delete));

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImage));

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
                Title = Product.Name;
                ImageSource = Product.ImageFullPath;
                IsEdit = true;
            }
            else
            {
                Product = new Product();
                Title = "New Product";
                ImageSource = "noImage";
                IsEdit = false;
            }
        }

        private async void Delete()
        {
            var confirm = await App.Current.MainPage.DisplayAlert(
                "Confirm",
                "Are you sure to delete the product?",
                "Yes",
                "No");
            if (!confirm)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.DeleteAsync(
                url,
                "/api",
                "/Products",
                this.Product.Id,
                "bearer",
                ProductsViewModel.GetInstance().TokenResponse.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await _navigationService.GoBackAsync();
        }

        private async void Save()
        {
            throw new NotImplementedException();
        }

        private async void ChangeImage()
        {
            throw new NotImplementedException();
        }
    }
}
