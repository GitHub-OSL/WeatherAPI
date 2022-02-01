using System.Text.Json.Serialization;

namespace WeatherApi.Dto.Models
{
    public class WeatherForecastLocation
    {
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        [JsonPropertyName("tz_id")]
        public string? TimeZone { get; set; }

        public DateTime LocalTime { get; set; }

    }
}
