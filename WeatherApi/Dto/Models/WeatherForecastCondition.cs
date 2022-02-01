using System.Text.Json.Serialization;

namespace WeatherApi.Dto.Models
{
    public class WeatherForecastCondition
    {
        [JsonPropertyName("text")]
        public string? Description { get; set; }

        //public Uri? Icon { get; set; }


    }
}
