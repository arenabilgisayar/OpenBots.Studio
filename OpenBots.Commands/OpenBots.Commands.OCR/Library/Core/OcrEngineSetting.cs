namespace TextXtractor.Ocr.Core
{
    public class OcrEngineSetting
    {
       

        public OcrEngineSetting()
        {
            IsServiceRatelimitEnabled = false;
        }

        public static OcrEngineSetting Create(ISettings settings)
        {
            OcrEngineSetting engineSetting = new OcrEngineSetting();
            engineSetting.RedisConnectionString = settings.GetValue("ENGINE_REDISCONNECTIONSTRING", "TextXtractor.Ocr.Engine:RedisConnectionString");
            engineSetting.Service = settings.GetValue("ENGINE_SERVICE", "TextXtractor.Ocr.Engine:Service");
            string isServiceRateLimitEnabledString = settings.GetValue("ENGINE_ISSERVICERATELIMITENABLED", "TextXtractor.Ocr.Engine:IsServiceRatelimitEnabled");
            bool isServiceRateLimitEnabled = false;
            bool.TryParse(isServiceRateLimitEnabledString, out isServiceRateLimitEnabled);
            engineSetting.IsServiceRatelimitEnabled = isServiceRateLimitEnabled;
            return engineSetting;
        }

        public string Service { get; set; }

        public string RedisConnectionString { get; set; }

        public bool IsServiceRatelimitEnabled { get; set; }

    }
}
