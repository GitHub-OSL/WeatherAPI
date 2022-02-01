using System.Net;
using WeatherApi;
using WeatherApi.Dto.Responses;
using WeatherApiCli;

Console.WriteLine("Initialising settings from arguments..");
var settings = new Settings();

try
{
    WeatherService weatherService;

    if (settings.UseProxy)
    {
        // TODO: consider credentials other than defaults
        var httpHandler = new HttpClientHandler { UseProxy = true, Proxy = null, DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials };
        weatherService = new WeatherService(settings.APIKey, httpHandler);
    }
    else
    {
        weatherService = new WeatherService(settings.APIKey);
    }

    using (weatherService)
    {
        var citySearchValue = settings.CityNameSearch;

        do
        {
            if (!string.IsNullOrWhiteSpace(citySearchValue))
            {
                Console.WriteLine("Calling WeatherAPi..");
                
                DisplayForcast(await weatherService.GetWeatherForecast(citySearchValue));
            }

            // Check for another city, or exit..
            Console.WriteLine("Enter city name:");
            citySearchValue = Console.ReadLine() ?? string.Empty;

            Console.Clear();
            Console.WriteLine("Enter 'Exit' to quit");

        } while (!citySearchValue.Equals("exit", StringComparison.OrdinalIgnoreCase));

    }
}
catch (Exception)
{

    throw;
}





static void DisplayForcast(WeatherForecastResponse weatherforecast)
{

    if (!string.IsNullOrWhiteSpace(weatherforecast.Error?.Message))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {weatherforecast.Error?.Message}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine("\n Location:");    
        Console.WriteLine($" - Name:      {weatherforecast.Location?.Name}");
        Console.WriteLine($" - Region:    {weatherforecast.Location?.Region}");
        Console.WriteLine($" - Country:   {weatherforecast.Location?.Country}");
        Console.WriteLine($" - Lat:       {weatherforecast.Location?.Lat}");
        Console.WriteLine($" - Long:      {weatherforecast.Location?.Lon}");
        Console.WriteLine($" - TimeZone:  {weatherforecast.Location?.TimeZone}");
        Console.WriteLine($" - LocalTime: {weatherforecast.Location?.LocalTime}");

        Console.WriteLine($"\n Current Conditions: {weatherforecast.Current?.Condition?.Description}");

        //TODO: Consider rolling over to the following day if there's not enough hours left in the day.
        var hoursToDisplay = weatherforecast.Forecast?.ForecastDay?.First()?.Hour?.Where(h => h.Time > weatherforecast.Location?.LocalTime).Select(h => $"{h.Time.ToString("HH:mm")}:  {h.Condition?.Description}").Take(3);
        if (hoursToDisplay != null)
        {
            Console.WriteLine($"\n Forecast:\n   {string.Join("\n   ", hoursToDisplay)}");
        }

        Console.WriteLine();
    }
}