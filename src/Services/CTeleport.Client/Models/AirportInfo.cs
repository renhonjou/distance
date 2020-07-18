using Newtonsoft.Json;

namespace CTeleport.Client.Models
{
    public class AirportInfo
    {
        [JsonProperty("iata")]
        public string IataCode { get; set; }
        [JsonProperty("location")]
        public LocationPoint Location { get; set; }
    }
}
