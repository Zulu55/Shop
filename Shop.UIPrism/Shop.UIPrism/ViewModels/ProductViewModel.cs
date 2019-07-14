using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Shop.Common.Helpers;
using Shop.Common.Models;
using Shop.Common.Services;
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
        private MediaFile _file;
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
                Product = new Product
                {
                    User = ProductsViewModel.GetInstance().User
                };

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
            if (string.IsNullOrEmpty(this.Product.Name))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a product name.",
                    "Accept");
                return;
            }

            if (this.Product.Price <= 0)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "The price must be a number greather than zero.",
                    "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (_file != null)
            {
                Product.ImageArray = FilesHelper.ReadFully(_file.GetStream());
            }

            var url = App.Current.Resources["UrlAPI"].ToString();
            Response response;

            if (Product.Id != 0)
            {
                response = await _apiService.PutAsync(
                    url,
                    "/api",
                    "/Products",
                    Product.Id,
                    Product,
                    "bearer",
                    ProductsViewModel.GetInstance().TokenResponse.Token);
            }
            else
            {
                response = await _apiService.PostAsync(
                    url,
                    "/api",
                    "/Products",
                    Product,
                    "bearer",
                    ProductsViewModel.GetInstance().TokenResponse.Token);
            }

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            await _navigationService.GoBackAsync();
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you take the picture?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From Camera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
        }
    }
}