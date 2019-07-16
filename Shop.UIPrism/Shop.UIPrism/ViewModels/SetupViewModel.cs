using Prism.Navigation;

namespace Shop.UIPrism.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        public SetupViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Setup";
        }
    }
}