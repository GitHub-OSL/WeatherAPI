
namespace WeatherApi.Dto.Models
{
    public class WeatherForecastHour
    {
        public DateTime Time { get; set; }
        public WeatherForecastCondition? Condition { get; set; }

    }
}
