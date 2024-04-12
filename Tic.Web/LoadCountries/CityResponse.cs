using Newtonsoft.Json;

namespace Tic.Web.LoadCountries
{
    public class CityResponse
    {
        [JsonProperty("id")]
        public long CityId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
