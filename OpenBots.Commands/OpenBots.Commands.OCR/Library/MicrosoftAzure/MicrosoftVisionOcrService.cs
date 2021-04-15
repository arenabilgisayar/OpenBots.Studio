using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using TextXtractor.Ocr.Core;
using System.Threading;
using System.Diagnostics;

namespace TextXtractor.Ocr.MicrosoftAzure
{
    internal class MicrosoftVisionOcrService : IOcrService
    {
        private const string _serviceName = "MicrosoftVisionOcrService";

        protected ServiceSettings<MicrosoftVisionOcrSetting> settings;

        public void Init()
        {
            
        }

        public string ServiceName
        {
            get { return _serviceName; }
        }

        public int SettingsCount()
        {
            return settings.Count;
        }
        // AUTHENTICATE
        //  Creates a Computer Vision client used by each example.
        protected ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public Core.OcrResult PerformRecognition(Stream imageStream, int settingIndex =0)
        {
            if (!settings.ContainsKey(settingIndex))
                throw new ArgumentOutOfRangeException();

            // Read the text from the local image
            MicrosoftVisionOcrSetting setting = settings[settingIndex];

            ComputerVisionClient client = Authenticate(setting.ServiceEndPoint, setting.SubscriptionKey);

            const int numberOfCharsInOperationId = 36;
            BatchReadFileInStreamHeaders localFileTextHeaders = client.BatchReadFileInStreamAsync(imageStream).Result;

            // Get the operation location (operation ID)
            string operationLocation = localFileTextHeaders.OperationLocation;

            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);



            // Extract text, wait for it to complete.
            int i = 0;
            int maxRetries = setting.MaxRetries;
            ReadOperationResult results;

            do
            {
                results = client.GetReadOperationResultAsync(operationId).Result;
                Thread.Sleep(setting.ServiceWaitTimeInMs);
                //Task.Delay(setting.ServiceWaitTimeInMs);
                if (i == maxRetries-1)
                {
                    Console.WriteLine("Server timed out.");
                }
            }

            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);


            var textRecognitionLocalFileResult = results.RecognitionResults.FirstOrDefault();
            if(textRecognitionLocalFileResult == null)
            {
                throw new CannotExtractContentException();
            }

            ConvertResponse convert = new ConvertResponse();
            var result =  convert.FromTextRecognitionResult(textRecognitionLocalFileResult);

            ContentExtractor extractor = new ContentExtractor();
            result.Text = extractor.GetExtractedTextWithSpaces(result);

            result.ServiceName = ServiceName;

            try
            {
                System.Reflection.Assembly assembly = typeof(ComputerVisionClient).Assembly;
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                result.ServiceVersion = fvi.FileVersion;
            }
            catch { }

            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public MicrosoftVisionOcrService(ServiceSettings<MicrosoftVisionOcrSetting> settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MicrosoftVisionOcrService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
