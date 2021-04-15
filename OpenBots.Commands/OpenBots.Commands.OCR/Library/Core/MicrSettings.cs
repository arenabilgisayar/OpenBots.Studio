using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class MicrSettings
    {
        public string ServiceUri { get; set; }
        public string ApiKey { get; set; }

        public MicrSettings()
        {
            ServiceUri = Environment.GetEnvironmentVariable("MICR_SERVICE_URI");
            ApiKey = Environment.GetEnvironmentVariable("API_KEY");
        }

        public MicrSettings(IDigitizationSettings settings)
        {
            if (settings != null)
            {
                ServiceUri = settings.Url;
                ApiKey = settings.ApiKey;
            }
        }
    }
}
