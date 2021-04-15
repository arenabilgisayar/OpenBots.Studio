using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.DrivingLicense
{
    public class DrivingLicenseService : IDrivingLicenseService
    {
        private DrivingLicenseSetting settings;
        public DrivingLicenseService(DrivingLicenseSetting settings)
        {
            this.settings = settings;
        }

        public void Dispose()
        {
        }

        public void Init()
        {
        }

        public DrivingLicenseInfo PerformRecognition(byte[] imageRawBytes)
        {
            DrivingLicenseInfo dlInfo = null;
            var serviceUri = settings.ServiceUri;
            var client = new RestClient(serviceUri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("file", imageRawBytes, "file");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                dlInfo = JsonConvert.DeserializeObject<DrivingLicenseInfo>(response.Content);
            }

            return dlInfo;
        }

        public DrivingLicenseInfo PerformRecognition(Stream imageRawBytes)
        {
            var memoryStream = (MemoryStream)imageRawBytes;
            byte[] imageByte = memoryStream.ToArray();

            return PerformRecognition(imageByte);
        }

    }
}
