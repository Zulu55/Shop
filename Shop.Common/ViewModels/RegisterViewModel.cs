namespace Shop.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Services;

    public class RegisterViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private List<Country> countries;
        private List<City> cities;
        private Country selectedCountry;
        private City selectedCity;
        private MvxCommand registerCommand;
        private string firstName;
        private string lastName;
        private string email;
        private string address;
        private string phone;
        private string password;
        private string confirmPassword;

        public RegisterViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.LoadCountries();
        }

        public ICommand RegisterCommand
        {
            get
            {
                this.registerCommand = this.registerCommand ?? new MvxCommand(this.RegisterUser);
                return this.registerCommand;
            }
        }

        public string FirstName
        {
            get => this.firstName;
            set => this.SetProperty(ref this.firstName, value);
        }

        public string LastName
        {
            get => this.lastName;
            set => this.SetProperty(ref this.lastName, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetProperty(ref this.email, value);
        }

        public string Address
        {
            get => this.address;
            set => this.SetProperty(ref this.address, value);
        }

        public string Phone
        {
            get => this.phone;
            set => this.SetProperty(ref this.phone, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set => this.SetProperty(ref this.confirmPassword, value);
        }

        public List<Country> Countries
        {
            get => this.countries;
            set => this.SetProperty(ref this.countries, value);
        }

        public List<City> Cities
        {
            get => this.cities;
            set => this.SetProperty(ref this.cities, value);
        }

        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                this.selectedCountry = value;
                this.RaisePropertyChanged(() => SelectedCountry);
                this.Cities = SelectedCountry.Cities;
            }
        }

        public City SelectedCity
        {
            get => selectedCity;
            set
            {
                selectedCity = value;
                RaisePropertyChanged(() => SelectedCity);
            }
        }

        private async void LoadCountries()
        {
            var response = await this.apiService.GetListAsync<Country>(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Countries");

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            this.Countries = (List<Country>)response.Result;
        }

        private async void RegisterUser()
        {
            // TODO: Make the local validations
            var request = new NewUserRequest
            {
                Address = this.Address,
                CityId = this.SelectedCity.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Password = this.Password,
                Phone = this.Phone
            };

            var response = await this.apiService.RegisterUserAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Account",
                request);

            this.dialogService.Alert("Ok", "The user was created succesfully, you must " +
                "confirm your user by the email sent to you and then you could login with " +
                "the email and password entered.", "Accept");

            await this.navigationService.Close(this);
        }
    }
}
