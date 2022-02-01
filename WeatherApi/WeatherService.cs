using System.Text.Json;
using System.Web;
using WeatherApi.Dto.Models;
using WeatherApi.Dto.Responses;

namespace WeatherApi
{
    public class WeatherService : IDisposable
    {
        public JsonSerializerOptions SerializerOptions { get; private set; }
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;


        public WeatherService(string apiKey) : this(apiKey, new HttpClientHandler()) { }

        public WeatherService(string apiKey, HttpClientHandler handler)
        {

            this._apiKey = HttpUtility.UrlEncode(apiKey) ?? throw new ArgumentNullException(nameof(apiKey));
            if (handler is null) throw new ArgumentNullException(nameof(handler));

            this.SerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new WeatherServiceDateTimeConverter() } };

            this._httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}"), // TODO: refactor to configuration
            };
        }

        public async Task<WeatherForecastResponse> GetWeatherForecast(string cityName, int days = 1)
        {
            if (string.IsNullOrWhiteSpace(cityName)) throw new ArgumentNullException(nameof(cityName));
            if (days < 1 || days > 10) throw new ArgumentOutOfRangeException(nameof(days));

            try
            {
                // Call Weather API
                var responseMsg = await _httpClient.GetAsync($"{_httpClient.BaseAddress}&q={HttpUtility.UrlEncode(cityName)}&days={days}"); // Threadsafe, no need for synclock
                var responseContent = await responseMsg.Content.ReadAsStreamAsync();
                
                // Deserialise response body
                var deserializedResponse = await JsonSerializer.DeserializeAsync<WeatherForecastResponse>(responseContent, this.SerializerOptions);

                return deserializedResponse ?? new WeatherForecastResponse() { Error = new WeatherForecastResponseError() { Message = "Unexpected error during deserialisation of Weather API response" } };
            }
            catch (HttpRequestException ex)
            {
                return new WeatherForecastResponse() { Error = new WeatherForecastResponseError() { Message = new Exception("Error calling external weather API", ex).ToString() } };
            }
            catch (JsonException ex)
            {
                return new WeatherForecastResponse() { Error = new WeatherForecastResponseError() { Message = new Exception("Error deserialising Json response from Weather API", ex).ToString() } };
            }
            catch (Exception ex) { 
                return new WeatherForecastResponse() { Error = new WeatherForecastResponseError() { Message = new Exception("Error, see inner exception for details", ex).ToString() } };
            }
        }

        void IDisposable.Dispose() => _httpClient.Dispose();
    }
}