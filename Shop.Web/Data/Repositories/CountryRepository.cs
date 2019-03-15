namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext context;

        public CountryRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Cities.Add(new City { Name = model.Name });
            this.context.Countries.Update(country);
            await this.context.SaveChangesAsync();
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            this.context.Cities.Remove(city);
            await this.context.SaveChangesAsync();
            return country.Id;
        }

        public IQueryable GetCountriesWithCities()
        {
            return this.context.Countries
                .Include(c => c.Cities)
                .OrderBy(c => c.Name);
        }

        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await this.context.Countries
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            this.context.Cities.Update(city);
            await this.context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await this.context.Cities.FindAsync(id);
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = this.context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCities(int conuntryId)
        {
            var country = this.context.Countries.Find(conuntryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = country.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;
        }

        public async Task<Country> GetCountryAsync(City city)
        {
            return await this.context.Countries
                .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
        }

    }
}
