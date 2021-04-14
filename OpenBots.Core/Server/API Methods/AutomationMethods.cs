using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using OpenBots.Server.SDK.Api;
using System;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class AutomationMethods
    {
        public static AutomationsApi apiInstance = new AutomationsApi(serverURL);

        public static void UploadAutomation(string token, string name, string filePath, string automationEngine)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    apiInstance.ApiVapiVersionAutomationsPostAsyncWithHttpInfo(apiVersion, name, _file, automationEngine).Wait();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AutomationsApi.ApiVapiVersionAutomationsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void UpdateParameters(RestClient client, Guid? automationId, IEnumerable<AutomationParameter> automationParameters)
        {
            var request = new RestRequest("api/v1/Automations/{automationId}/UpdateParameters", Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddUrlSegment("automationId", automationId.ToString());
            request.AddJsonBody(automationParameters);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }
    }
}
