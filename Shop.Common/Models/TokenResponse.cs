namespace Shop.Common.Models
{
    using System;
    using Newtonsoft.Json;

    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }
    }
}
