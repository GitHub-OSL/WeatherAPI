
namespace WeatherApi.Dto.Models
{
    public class WeatherForecastDay
    {

        public DateTime Date { get; set; }

        public WeatherForecastDaySummary? Day { get; set; }

        public List<WeatherForecastHour>? Hour { get; set; }
    }
}
