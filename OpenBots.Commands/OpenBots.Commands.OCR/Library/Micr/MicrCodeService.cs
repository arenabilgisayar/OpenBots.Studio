using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextXtractor.Ocr.Core;
using RestSharp;
using Newtonsoft.Json;

namespace TextXtractor.Ocr.Micr
{
    public class MicrCodeService : IMicrCodeService
    {
        private MicrSettings settings;
        public MicrCodeService(MicrSettings settings)
        {
            this.settings = settings;
        }

        public void Dispose()
        {
        }

        public void Init()
        {
        }

        public MicrCode PerformRecognition(byte[] imageRawBytes)
        {
            var serviceUri = settings.ServiceUri;
            var client = new RestClient(serviceUri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("file", imageRawBytes, "file");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var micr = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);

                MicrCode micrCode = new MicrCode()
                {
                    AccountNumber = micr["account_number"],
                    CheckNumber = micr["check_number"],
                    RoutingNumber = micr["routing_number"]
                };

                return micrCode;
            }

            return null;
        }

        public MicrCode PerformRecognition(Stream imageRawBytes)
        {
            var memoryStream = (MemoryStream)imageRawBytes;
            byte[] imageByte = memoryStream.ToArray();

            return PerformRecognition(imageByte);
        }

    }
}
