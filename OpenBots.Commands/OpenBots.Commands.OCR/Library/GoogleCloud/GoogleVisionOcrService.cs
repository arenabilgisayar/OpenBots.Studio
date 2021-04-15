using Google.Cloud.Vision.V1;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Diagnostics;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.GoogleCloud
{
    internal class GoogleVisionOcrService : IOcrService
    {
        private const string _serviceName = "GoogleVisionOcrService";

        protected ServiceSettings<GoogleVisionOcrSetting> settings;

        public GoogleVisionOcrService(ServiceSettings<GoogleVisionOcrSetting> settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Init()
        {

        }

        public string ServiceName
        {
            get { return _serviceName; }
        }


        public ImageAnnotatorClient Client { get; set; }

        public int SettingsCount()
        {
            return settings.Count;
        }

        public TextAnnotation OcrRequest(Stream imageStream, int settingIndex = 0)
        {

            if (!settings.ContainsKey(settingIndex))
                throw new ArgumentOutOfRangeException(string.Format("There are no Configuration {0} for GoogleVisionOcrService", settingIndex));

            GoogleVisionOcrSetting setting = settings[settingIndex];

            TextAnnotation results = null;
            Stopwatch gcStopwatch = new Stopwatch();
            gcStopwatch.Start();

            if (!string.IsNullOrEmpty(setting.GetKeyAbsolutePath()))
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", setting.GetKeyAbsolutePath());

            if (!string.IsNullOrEmpty(setting.HttpProxyHost?.Trim()))
            {

                string httpProxy = string.Format("{0}:{1}", setting.HttpProxyHost.Trim(), setting.HttpProxyPort.Trim());
                if (!httpProxy.ToLower().StartsWith("http"))
                    httpProxy = string.Format("http://{0}", httpProxy);

                Uri proxyHostUri = new Uri(httpProxy);

                Environment.SetEnvironmentVariable("http_proxy", proxyHostUri.AbsoluteUri);
                Environment.SetEnvironmentVariable("https_proxy", proxyHostUri.AbsoluteUri);
                Environment.SetEnvironmentVariable("https.proxyHost", proxyHostUri.Host);
                Environment.SetEnvironmentVariable("https.proxyPort", proxyHostUri.Port.ToString());
            }
            Client = ImageAnnotatorClient.Create();
            var image = Google.Cloud.Vision.V1.Image.FromStream(imageStream);
            results = Client.DetectDocumentTextAsync(image).Result;


          
            gcStopwatch.Stop();

            return results;
        }

        public OcrResult PerformRecognition(Stream imageSteam, int settingIndex = 0)
        {
            TextAnnotation annotation = OcrRequest(imageSteam, settingIndex);

           // File.WriteAllText(@"d:\googleRaw.json", JsonConvert.SerializeObject(annotation));

            ConvertResponse convert = new ConvertResponse();

            OcrResult result = convert.FromAnnotation(annotation);

            result.ServiceName = ServiceName;

            try
            {
                System.Reflection.Assembly assembly = typeof(ImageAnnotatorClient).Assembly;
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                result.ServiceVersion = fvi.FileVersion;
            }
            catch { }

            var page = OcrEngine.ExtractPageNumber(result.Text);
            result.DetectedPageNumber = page.Item1;
            result.DetectedPageNumberOf = page.Item2;


            return result;
        }

        public void Dispose()
        {
          
        }

      
    }
}
