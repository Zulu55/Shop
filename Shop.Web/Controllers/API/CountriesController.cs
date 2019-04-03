namespace Shop.Web.Controllers.API
{
    using Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            return Ok(this.countryRepository.GetCountriesWithCities());
        }
    }
}
