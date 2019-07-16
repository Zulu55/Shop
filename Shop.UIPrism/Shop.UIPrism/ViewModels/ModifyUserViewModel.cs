using Prism.Navigation;

namespace Shop.UIPrism.ViewModels
{
    public class ModifyUserViewModel : ViewModelBase
    {
        public ModifyUserViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Modify User";
        }
    }
}
