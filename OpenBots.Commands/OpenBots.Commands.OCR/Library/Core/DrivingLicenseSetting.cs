using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class DrivingLicenseSetting

    {
        public string ServiceUri { get; set; }
        public string ApiKey { get; set; }

        public DrivingLicenseSetting()
        {
            ServiceUri = Environment.GetEnvironmentVariable("DL_SERVICE_URI");
            ApiKey = Environment.GetEnvironmentVariable("DL_API_KEY");
        }

        public DrivingLicenseSetting(IDigitizationSettings settings)
        {
            if (settings != null)
            {
                ServiceUri = settings.Url;
                ApiKey = settings.ApiKey;
            }
        }
    }
}
