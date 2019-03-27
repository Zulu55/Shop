namespace Shop.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class Product
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("lastPurchase")]
        public DateTime? LastPurchase { get; set; }

        [JsonProperty("lastSale")]
        public DateTime? LastSale { get; set; }

        [JsonProperty("isAvailabe")]
        public bool IsAvailabe { get; set; }

        [JsonProperty("stock")]
        public long Stock { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("imageFullPath")]
        public string ImageFullPath { get; set; }

        public override string ToString()
        {
            return $"{this.Name} {this.Price:C2}";
        }
    }
}
