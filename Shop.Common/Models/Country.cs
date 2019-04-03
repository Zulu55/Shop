namespace Shop.Common.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Country
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cities")]
        public List<City> Cities { get; set; }

        [JsonProperty("numberCities")]
        public int NumberCities { get; set; }
    }
}
