using System;
using System.Collections.Generic;
using System.IO;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.GoogleCloud
{
    public class GoogleVisionOcrSetting 
    {
        public GoogleVisionOcrSetting() : base()
        {
            HttpProxyPort = "80";

           
        }

        public string GetKeyAbsolutePath()
        {
            if (ApiPrivateKeyPath.Contains(@":\") || ApiPrivateKeyPath.Contains(@":/"))
            {
                return ApiPrivateKeyPath;
            }
            else
            {
                FileInfo executingExe = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var absolute_path = Path.Combine(executingExe.DirectoryName, ApiPrivateKeyPath);
                return Path.GetFullPath((new Uri(absolute_path)).LocalPath);
            }
        }


        public static ServiceSettings<GoogleVisionOcrSetting> Create(ISettings settings)
        {
            ServiceSettings<GoogleVisionOcrSetting> serviceSettings = new ServiceSettings<GoogleVisionOcrSetting>();
            int numOfConfigs = 0;

            string apiPrivateKeyPath = settings.GetValue("GOOGLE_APIPRIVATEKEYPATH", "TextXtractor.Ocr.Google:ApiPrivateKeyPath");
            if (!string.IsNullOrEmpty(apiPrivateKeyPath))
            {
                GoogleVisionOcrSetting serviceSetting = new GoogleVisionOcrSetting();
                serviceSetting.ApiPrivateKeyPath = apiPrivateKeyPath;
                serviceSetting.HttpProxyHost = settings.GetValue("GOOGLE_HTTPPROXYHOST", "TextXtractor.Ocr.Google:HttpProxyHost");
                serviceSetting.HttpProxyPort = settings.GetValue("GOOGLE_HTTPPROXYPORT", "TextXtractor.Ocr.Google:HttpProxyPort");
                serviceSettings.Add(numOfConfigs++,serviceSetting);
            }

         
            int maxConfigs = 100;
            do
            {
                apiPrivateKeyPath = settings.GetValue(
                    string.Format("GOOGLE_{0}_APIPRIVATEKEYPATH", numOfConfigs),
                    string.Format("TextXtractor.Ocr.Google{0}:ApiPrivateKeyPath", numOfConfigs));
                if (!string.IsNullOrEmpty(apiPrivateKeyPath))
                {
                    GoogleVisionOcrSetting serviceSetting = new GoogleVisionOcrSetting();
                    serviceSetting.ApiPrivateKeyPath = apiPrivateKeyPath;
                    serviceSetting.HttpProxyHost = settings.GetValue(
                        string.Format("GOOGLE_{0}_HTTPPROXYHOST", numOfConfigs),
                        string.Format("TextXtractor.Ocr.Google{0}:HttpProxyHost", numOfConfigs));
                    serviceSetting.HttpProxyPort = settings.GetValue(
                        string.Format("GOOGLE_{0}_HTTPPROXYPORT", numOfConfigs),
                        "TextXtractor.Ocr.Google:HttpProxyPort");
                    serviceSettings.Add(numOfConfigs, serviceSetting);
                }
                else
                    break;
            }
            while (numOfConfigs++ < maxConfigs);

            return serviceSettings;
        }


        public string HttpProxyHost { get; set; }
        public string HttpProxyPort { get; set; }

        public string ApiPrivateKeyPath { get; set; }
    }
}
