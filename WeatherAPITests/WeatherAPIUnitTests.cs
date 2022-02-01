using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WeatherApi;
using WeatherApi.Dto.Responses;

namespace WeatherAPITests
{
    [TestClass]
    public class WeatherAPIUnitTests
    {
        private static readonly string _apiKey = "37ab0c7ae23f4c00ad3232249222801";

        [TestMethod]
        public void TestInvalidAPIKey()
        {

            using var weatherService = new WeatherService("INVALID API KEY");
            var result = weatherService.GetWeatherForecast("London").Result;

            Assert.IsNotNull(result?.Error?.Message, "No Error message for API Call with invalid API Key");
        }

        [TestMethod]
        public void TestInvalidFormattedAPIKey()
        {

            using var weatherService = new WeatherService("INVALID F@RMATTED API KEY");
            var result = weatherService.GetWeatherForecast("London").Result;

            Assert.IsNotNull(result?.Error?.Message, "No Error message for API Call with invalid API Key");
        }

        [TestMethod]
        public void TestNullCity()
        {

            using var weatherService = new WeatherService(_apiKey);

            Assert.ThrowsException<ArgumentNullException>(() => throw weatherService.GetWeatherForecast(null).Exception.InnerExceptions[0], "No Error message for API Call with null city");
        }

        [TestMethod]
        public void TestInvalidFormatCity()
        {

            using var weatherService = new WeatherService(_apiKey);
            var result = weatherService.GetWeatherForecast("!£$%^&*()").Result;

            Assert.IsNotNull(result?.Error?.Message, "No Error message for API Call with invalid formatted city");
        }

        [TestMethod]
        public void TestUnknownCity()
        {

            using var weatherService = new WeatherService(_apiKey);
            var result = weatherService.GetWeatherForecast("LondonParisSydney").Result;

            Assert.IsNotNull(result?.Error?.Message, "No Error message for API Call with unknown city name");
        }

        [TestMethod]
        public void TestExpectedNumberOfDays1()
        {

            using var weatherService = new WeatherService(_apiKey);
            var result = weatherService.GetWeatherForecast("London").Result;

            Assert.IsTrue(result?.Forecast?.ForecastDay?.Count == 1, "GetWeatherForecast returned unexpeced number of days (1 expected)");
        }

        [TestMethod]
        public void TestExpectedNumberOfDays2()
        {

            using var weatherService = new WeatherService(_apiKey);
            var result = weatherService.GetWeatherForecast("London", 2).Result;

            Assert.IsTrue(result?.Forecast?.ForecastDay?.Count == 2, "GetWeatherForecast returned unexpeced number of days (2 expected)");
        }

        [TestMethod]
        public void TestWeatherAPIAutoCorrect()
        {

            using var weatherService = new WeatherService(_apiKey);
            var result = weatherService.GetWeatherForecast("Lon").Result;

            Assert.AreEqual<string>(result?.Location?.Name??string.Empty, "London");
        }
    }
}