using Prism.Navigation;
using Shop.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Shop.UIPrism.ViewModels
{
    public class MyMasterDetailViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MyMasterDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_info",
                    PageName = "Products",
                    Title = "Products"
                },

                new Menu
                {
                    Icon = "ic_info",
                    PageName = "About",
                    Title = "About"
                },

                new Menu
                {
                    Icon = "ic_person",
                    PageName = "ModifyUser",
                    Title = "Modify User"
                },

                new Menu
                {
                    Icon = "ic_phonelink_setup",
                    PageName = "Setup",
                    Title = "Setup"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = "Login",
                    Title = "Log out"
                }
            };

            this.Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title
                }).ToList());
        }
    }
}
