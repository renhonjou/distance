using Newtonsoft.Json;

namespace CTeleport.Client.Models
{
    public class LocationPoint
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lon")]
        public double Lon { get; set; }
    }
}
