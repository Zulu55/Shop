using Prism.Navigation;

namespace Shop.UIPrism.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "About";
        }
    }
}
