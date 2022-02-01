using System.Text.Json.Serialization;
using WeatherApi.Dto.Models;

namespace WeatherApi.Dto.Responses
{
    public class WeatherForecastResponse
    {
        public WeatherForecastResponseError? Error { get; set; }

        public WeatherForecastLocation? Location { get; set; }

        public WeatherForecastCurrent? Current{ get; set; }

        public WeatherForecast? Forecast { get; set; }

    }
}
