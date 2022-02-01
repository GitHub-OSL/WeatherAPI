using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApiCli
{
    internal class Settings
    {
        public string APIKey { get; set; }
        public string CityNameSearch { get; set; }

        private bool _useProxy;
        public bool UseProxy
        {
            get { return _useProxy; }
            set { _useProxy = value; }
        }


        public Settings()
        {
            var args = Environment.GetCommandLineArgs().ToList();

            APIKey = GetOrDefaultSetting(args, "APIKey");
            
            bool.TryParse( GetOrDefaultSetting(args, "UseProxy"), out _useProxy);

            CityNameSearch = GetSetting(args, "CityNameSearch");

        }

        private static string GetSetting(List<string> args, string argumentName)
        {
            // Check if the argumant has been passed (formatted -[argument name])
            var index = args.FindIndex(x => x.Equals($"-{argumentName}", StringComparison.OrdinalIgnoreCase));
            string? val = null;

            if (index >= 0 && (index++) < args.Count)
            {
                val = args[index];
            }

            return val ?? string.Empty;
        }

        private static string GetOrDefaultSetting(List<string> args, string argumentName) => 
            ValidateSetting(GetSetting(args, argumentName), argumentName);

        static string ValidateSetting(string? value, string description)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine($"{description} is required: ");
                value = Console.ReadLine();

                return ValidateSetting(value, description);
            }

            return value;
        }

    }
}
