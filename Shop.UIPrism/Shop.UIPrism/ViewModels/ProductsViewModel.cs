using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Shop.Common.Helpers;
using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Shop.UIPrism.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<ProductItemViewModel> _products;
        private DelegateCommand _addProductCommand;

        public ProductsViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Products";
            LoadProductsAsycn();
        }

        public DelegateCommand AddProductCommand => _addProductCommand ?? (_addProductCommand = new DelegateCommand(AddProduct));

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private async void LoadProductsAsycn()
        {
            IsRefreshing = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                token.Token);

            IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            var myProducts = (List<Product>)response.Result;
            Products = new ObservableCollection<ProductItemViewModel>(
                myProducts.Select(p => new ProductItemViewModel(_navigationService)
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    ImageFullPath = p.ImageFullPath,
                    IsAvailabe = p.IsAvailabe,
                    LastPurchase = p.LastPurchase,
                    LastSale = p.LastSale,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    User = p.User
                })
            .OrderBy(p => p.Name)
            .ToList());
        }

        private async void AddProduct()
        {
            await _navigationService.NavigateAsync("Product");
        }
    }
}
