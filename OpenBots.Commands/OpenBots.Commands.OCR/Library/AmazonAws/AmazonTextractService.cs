using Amazon.Runtime;
using Amazon.Textract;
using Amazon.Textract.Model;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.AmazonAws
{
    internal class AmazonTextractService : IOcrService
    {

        protected ServiceSettings<AmazonTextractSetting> settings;

        private const string _serviceName = "AmazonTextractService";
        //protected AmazonTextractSetting setting;

        //public string ExtractText(OcrResult result)
        //{
        //    return result.Text;
        //}

        public void Init()
        {
           
        }
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public OcrResult PerformRecognition(Stream imageSteam, int settingIndex = 0)
        {
            if (!settings.ContainsKey(settingIndex))
                throw new ArgumentOutOfRangeException();

            AmazonTextractSetting setting = settings[settingIndex];

            var credentials = new BasicAWSCredentials(setting.AwsAccessKeyId, setting.AwsSecretAccessKey);
            var endpoint = Amazon.RegionEndpoint.GetBySystemName(setting.AwsRegion);
            AmazonTextractClient client = new AmazonTextractClient(credentials, endpoint);
            
            DetectDocumentTextRequest request = new DetectDocumentTextRequest();
            request.Document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            CopyStream(imageSteam, memoryStream);
            request.Document.Bytes = memoryStream;
            DetectDocumentTextResponse response = client.DetectDocumentTextAsync(request).Result;

            if (response.HttpStatusCode != HttpStatusCode.OK
                || response == null
                || response.Blocks == null
                || response.Blocks.Count == 0)
            {
                throw new CannotExtractContentException();
            }

         


            ConvertResponse convert = new ConvertResponse(imageSteam);
            var result = convert.FromDetectDocumentTextResponse(response);
            result.Text = convert.ExtractText(result);

            result.ServiceName = ServiceName;

            try
            {
                System.Reflection.Assembly assembly = typeof(AmazonTextractClient).Assembly;
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                result.ServiceVersion = fvi.FileVersion;
            }
            catch { }

            return result;


        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public AmazonTextractService(ServiceSettings<AmazonTextractSetting> settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }


        public int SettingsCount()
        {
            return settings.Count;
        }

        public string ServiceName
        {
            get { return _serviceName; }
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
        // ~AmazonTextractService()
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
