using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.MicrosoftAzure
{
    public class MicrosoftVisionOcrSetting
    {

        public MicrosoftVisionOcrSetting()
        {
            SubscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
            ServiceEndPoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
            MaxRetries = 30;
            ServiceWaitTimeInMs = 1000;
        }

        public MicrosoftVisionOcrSetting(string subscriptionKey, string serviceEndPoint = "")
        {
            SubscriptionKey = subscriptionKey;
            ServiceEndPoint = serviceEndPoint;
            if (string.IsNullOrEmpty(serviceEndPoint))
                ServiceEndPoint = @"https://westus2.api.cognitive.microsoft.com/vision/v2.0/ocr";
        }

        public string SubscriptionKey { get; set; }

        public string ServiceEndPoint { get; set; }


        public int MaxRetries { get; set; }

        public int ServiceWaitTimeInMs { get; set; }

        public static ServiceSettings<MicrosoftVisionOcrSetting> Create(ISettings settings)
        {
            ServiceSettings<MicrosoftVisionOcrSetting> serviceSettings = new ServiceSettings<MicrosoftVisionOcrSetting>();
            int numOfConfigs = 0;

            string subscriptionKey = settings.GetValue("MICROSOFT_SUBSCRIPTIONKEY", "TextXtractor.Ocr.Microsoft:SubscriptionKey");
            if (!string.IsNullOrEmpty(subscriptionKey))
            {
                MicrosoftVisionOcrSetting serviceSetting = new MicrosoftVisionOcrSetting();
                serviceSetting.SubscriptionKey = subscriptionKey;
                serviceSetting.ServiceEndPoint = settings.GetValue("MICROSOFT_SERVICEENDPOINT", "TextXtractor.Ocr.Microsoft:ServiceEndPoint");
                int maxTries = 30;
                if(int.TryParse(settings.GetValue("MICROSOFT_MAXRETRIES", "TextXtractor.Ocr.Microsoft:MaxRetries"), out maxTries))
                    serviceSetting.MaxRetries = maxTries;

                int serviceWaitTimeInMs = 1000;
                if( int.TryParse(settings.GetValue("SERVICEWAITTIMEINMS", "TextXtractor.Ocr.Microsoft:ServiceWaitTimeInMs"), out serviceWaitTimeInMs))
                    serviceSetting.ServiceWaitTimeInMs = serviceWaitTimeInMs;

                serviceSettings.Add(numOfConfigs++, serviceSetting);
            }

            return serviceSettings;
        }

    }
}
