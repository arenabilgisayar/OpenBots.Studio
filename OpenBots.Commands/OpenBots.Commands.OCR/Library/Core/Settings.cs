using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class Settings : ISettings
    {
        protected IConfiguration primaryConfiguration;
        protected IConfiguration fallbackConfiguration;

        public const string PREFIX = "TEXTXTRACTOR_OCR_";

        public Settings(IConfiguration primaryConfiguration, IConfiguration fallbackConfiguration = null)
        {
            this.primaryConfiguration = primaryConfiguration ?? throw new ArgumentNullException(nameof(primaryConfiguration));
            this.fallbackConfiguration = fallbackConfiguration;
        }

        public Settings(string prefix = PREFIX, string configurationFile = "")
        {
            primaryConfiguration = CreateEnvironmentConfiguration(prefix);

            if (!string.IsNullOrEmpty(configurationFile))
                fallbackConfiguration = CreateJsonConfiguration(configurationFile);
        }

        public string GetValue(string primaryKey, string fallbackKey = "")
        {
            if (primaryConfiguration != null && !string.IsNullOrEmpty(primaryConfiguration[primaryKey]))
                return primaryConfiguration[primaryKey];

            if (fallbackConfiguration != null)
            {
                if (string.IsNullOrEmpty(fallbackKey) && !string.IsNullOrEmpty(fallbackConfiguration[primaryKey]))
                {
                    return fallbackConfiguration[primaryKey];
                }
                else
                {
                    return fallbackConfiguration[fallbackKey];
                }
            }

            return string.Empty;
        }

        

        public static IConfiguration CreateJsonConfiguration(string configurationFile = "")
        {
            IConfigurationBuilder configuration = new ConfigurationBuilder();
            configuration = configuration.SetBasePath(Directory.GetCurrentDirectory());
            if(!string.IsNullOrEmpty(configurationFile))
                configuration = configuration.AddJsonFile(configurationFile);
            return configuration.Build();
        }

        public static IConfiguration CreateEnvironmentConfiguration(string prefix = "TEXTXTRACTOR_OCR_")
        {
            IConfigurationBuilder configuration = new ConfigurationBuilder();
            configuration = configuration.AddEnvironmentVariables(prefix); //
            return configuration.Build();
        }
    }
}
