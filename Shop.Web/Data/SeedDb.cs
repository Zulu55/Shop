namespace Shop.Web.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            await this.CheckRoles();

            if (!this.context.Countries.Any())
            {
                await this.CountriesAndCities();
            }

            await this.CheckUser("brad@gmail.com", "Brad", "Pit", "Customer");
            await this.CheckUser("angelina@gmail.com", "Angelina", "Jolie", "Customer");
            var user = await this.CheckUser("jzuluaga55@gmail.com", "Juan", "Zuluaga", "Admin");

            // Add products
            if (!this.context.Products.Any())
            {
                this.AddProduct("AirPods", 159, user);
                this.AddProduct("Blackmagic eGPU Pro", 1199, user);
                this.AddProduct("iPad Pro", 799, user);
                this.AddProduct("iMac", 1398, user);
                this.AddProduct("iPhone X", 749, user);
                this.AddProduct("iWatch Series 4", 399, user);
                this.AddProduct("Mac Book Air", 789, user);
                this.AddProduct("Mac Book Pro", 1299, user);
                this.AddProduct("Mac Mini", 708, user);
                this.AddProduct("Mac Pro", 2300, user);
                this.AddProduct("Magic Mouse", 47, user);
                this.AddProduct("Magic Trackpad 2", 140, user);
                this.AddProduct("USB C Multiport", 145, user);
                this.AddProduct("Wireless Charging Pad", 67.67M, user);
                await this.context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUser(string userName, string firstName, string lastName, string role)
        {
            // Add user
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                user = await this.AddUser(userName, firstName, lastName, role);
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task<User> AddUser(string userName, string firstName, string lastName, string role)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = userName,
                UserName = userName,
                Address = "Calle Luna Calle Sol",
                PhoneNumber = "350 634 2747",
                CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
            };

            var result = await this.userHelper.AddUserAsync(user, "123456");
            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException("Could not create the user in seeder");
            }

            await this.userHelper.AddUserToRoleAsync(user, role);
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);
            return user;
        }

        private async Task CountriesAndCities()
        {
            var citiesCol = new List<City>();
            citiesCol.Add(new City { Name = "Medellín" });
            citiesCol.Add(new City { Name = "Bogotá" });
            citiesCol.Add(new City { Name = "Calí" });
            citiesCol.Add(new City { Name = "Barranquilla" });
            citiesCol.Add(new City { Name = "Bucaramanga" });
            citiesCol.Add(new City { Name = "Cartagena" });
            citiesCol.Add(new City { Name = "Pereira" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesCol,
                Name = "Colombia"
            });

            var citiesArg = new List<City>();
            citiesArg.Add(new City { Name = "Córdoba" });
            citiesArg.Add(new City { Name = "Buenos Aires" });
            citiesArg.Add(new City { Name = "Rosario" });
            citiesArg.Add(new City { Name = "Tandil" });
            citiesArg.Add(new City { Name = "Salta" });
            citiesArg.Add(new City { Name = "Mendoza" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesArg,
                Name = "Argentina"
            });

            var citiesUsa = new List<City>();
            citiesUsa.Add(new City { Name = "New York" });
            citiesUsa.Add(new City { Name = "Los Ángeles" });
            citiesUsa.Add(new City { Name = "Chicago" });
            citiesUsa.Add(new City { Name = "Washington" });
            citiesUsa.Add(new City { Name = "San Francisco" });
            citiesUsa.Add(new City { Name = "Miami" });
            citiesUsa.Add(new City { Name = "Boston" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesUsa,
                Name = "Estados Unidos"
            });

            var citiesEcuador = new List<City>();
            citiesEcuador.Add(new City { Name = "Quito" });
            citiesEcuador.Add(new City { Name = "Guayaquil" });
            citiesEcuador.Add(new City { Name = "Ambato" });
            citiesEcuador.Add(new City { Name = "Manta" });
            citiesEcuador.Add(new City { Name = "Loja" });
            citiesEcuador.Add(new City { Name = "Santo" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesEcuador,
                Name = "Ecuador"
            });

            var citiesPeru = new List<City>();
            citiesPeru.Add(new City { Name = "Lima" });
            citiesPeru.Add(new City { Name = "Arequipa" });
            citiesPeru.Add(new City { Name = "Cusco" });
            citiesPeru.Add(new City { Name = "Trujillo" });
            citiesPeru.Add(new City { Name = "Chiclayo" });
            citiesPeru.Add(new City { Name = "Iquitos" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesPeru,
                Name = "Peru"
            });

            var citiesChile = new List<City>();
            citiesChile.Add(new City { Name = "Santiago" });
            citiesChile.Add(new City { Name = "Valdivia" });
            citiesChile.Add(new City { Name = "Concepcion" });
            citiesChile.Add(new City { Name = "Puerto Montt" });
            citiesChile.Add(new City { Name = "Temucos" });
            citiesChile.Add(new City { Name = "La Sirena" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesChile,
                Name = "Chile"
            });

            var citiesUruguay = new List<City>();
            citiesUruguay.Add(new City { Name = "Montevideo" });
            citiesUruguay.Add(new City { Name = "Punta del Este" });
            citiesUruguay.Add(new City { Name = "Colonia del Sacramento" });
            citiesUruguay.Add(new City { Name = "Las Piedras" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesUruguay,
                Name = "Uruguay"
            });

            var citiesBolivia = new List<City>();
            citiesBolivia.Add(new City { Name = "La Paz" });
            citiesBolivia.Add(new City { Name = "Sucre" });
            citiesBolivia.Add(new City { Name = "Potosi" });
            citiesBolivia.Add(new City { Name = "Cochabamba" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesBolivia,
                Name = "Bolivia"
            });

            var citiesVenezuela = new List<City>();
            citiesVenezuela.Add(new City { Name = "Caracas" });
            citiesVenezuela.Add(new City { Name = "Valencia" });
            citiesVenezuela.Add(new City { Name = "Maracaibo" });
            citiesVenezuela.Add(new City { Name = "Ciudad Bolivar" });
            citiesVenezuela.Add(new City { Name = "Maracay" });
            citiesVenezuela.Add(new City { Name = "Barquisimeto" });

            this.context.Countries.Add(new Country
            {
                Cities = citiesVenezuela,
                Name = "Venezuela"
            });

            await this.context.SaveChangesAsync();
        }

        private async Task CheckRoles()
        {
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");
        }

        private void AddProduct(string name, decimal price, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = price,
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user,
                ImageUrl = $"~/images/Products/{name}.png"
            });
        }
    }
}