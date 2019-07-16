using Prism.Commands;
using Prism.Navigation;
using Shop.Common.Models;

namespace Shop.UIPrism.ViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenu));

        private async void SelectMenu()
        {
            if (PageName.Equals("Login"))
            {
                await _navigationService.NavigateAsync("/NavigationPage/Login");
                return;
            }

            await _navigationService.NavigateAsync($"/MyMasterDetail/NavigationPage/{PageName}");

        }
    }

}
