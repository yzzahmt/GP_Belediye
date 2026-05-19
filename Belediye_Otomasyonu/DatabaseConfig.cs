using System;
using System.Configuration;

namespace Belediye_Otomasyonu
{
    internal static class DatabaseConfig
    {
        private static string Get(string name)
        {
            var settings = ConfigurationManager.ConnectionStrings[name];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    "App.config içinde connectionStrings bölümünde '" + name + "' adlı bağlantı tanımlı değil.");
            }
            return settings.ConnectionString;
        }

        public static string MySqlBelediye => Get("MySqlBelediye");
    }
}
