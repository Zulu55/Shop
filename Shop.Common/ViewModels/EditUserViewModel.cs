namespace Shop.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Interfaces;
    using Models;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Services;
    using Shop.Common.Helpers;

    public class EditUserViewModel : MvxViewModel
    {
        private readonly IApiService apiService;
        private readonly IMvxNavigationService navigationService;
        private readonly IDialogService dialogService;
        private List<Country> countries;
        private List<City> cities;
        private Country selectedCountry;
        private City selectedCity;
        private MvxCommand saveCommand;
        private User user;
        private bool isLoading;

        public EditUserViewModel(
            IMvxNavigationService navigationService,
            IApiService apiService,
            IDialogService dialogService)
        {
            this.apiService = apiService;
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.IsLoading = false;
            this.User = JsonConvert.DeserializeObject<User>(Settings.User);
            this.LoadCountries();
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.SetProperty(ref this.isLoading, value);
        }

        public ICommand SaveCommand
        {
            get
            {
                this.saveCommand = this.saveCommand ?? new MvxCommand(this.Save);
                return this.saveCommand;
            }
        }

        public User User
        {
            get => this.user;
            set => this.SetProperty(ref this.user, value);
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
            this.IsLoading = true;

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
            foreach (var country in this.Countries)
            {
                var city = country.Cities.Where(c => c.Id == this.User.CityId).FirstOrDefault();
                if (city != null)
                {
                    this.SelectedCountry = country;
                    this.SelectedCity = city;
                    break;
                }
            }

            this.IsLoading = false;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                this.dialogService.Alert("Error", "You must enter a first name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.LastName))
            {
                this.dialogService.Alert("Error", "You must enter a last name.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.Address))
            {
                this.dialogService.Alert("Error", "You must enter an address.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.User.PhoneNumber))
            {
                this.dialogService.Alert("Error", "You must enter a phone.", "Accept");
                return;
            }

            this.IsLoading = true;

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var response = await this.apiService.PutAsync(
                "https://shopzulu.azurewebsites.net",
                "/api",
                "/Account",
                this.User,
                "bearer",
                token.Token);

            this.IsLoading = true;

            if (!response.IsSuccess)
            {
                this.dialogService.Alert("Error", response.Message, "Accept");
                return;
            }

            Settings.User = JsonConvert.SerializeObject(this.User);
            await this.navigationService.Close(this);
        }
    }
}