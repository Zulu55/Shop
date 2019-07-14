using Prism.Commands;
using Prism.Navigation;
using Shop.Common.Models;

namespace Shop.UIPrism.ViewModels
{
    public class ProductItemViewModel : Product
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProductCommand;

        public ProductItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectProductCommand => _selectProductCommand ?? (_selectProductCommand = new DelegateCommand(SelectProduct));

        private async void SelectProduct()
        {
            var parameters = new NavigationParameters
            {
                { "product", this }
            };

            await _navigationService.NavigateAsync("Product", parameters);
        }
    }
}
