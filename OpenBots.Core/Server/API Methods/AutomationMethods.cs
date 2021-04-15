using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using System;
using System.Collections.Generic;
using OpenBots.Server.SDK.Api;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class AutomationMethods
    {
        public static AutomationsApi apiInstance = new AutomationsApi(serverURL);

        public static Automation UploadAutomation(string token, string name, string filePath, string automationEngine)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var result = apiInstance.ApiVapiVersionAutomationsPostWithHttpInfo(apiVersion, name, _file, automationEngine);
                    string automationString = JsonConvert.SerializeObject(result);
                    var automation = JsonConvert.DeserializeObject<Automation>(automationString);
                    return automation;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AutomationsApi.ApiVapiVersionAutomationsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void UpdateParameters(string token, Guid? automationId, IEnumerable<AutomationParameter> automationParameters)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var automationParametersList = new List<OpenBots.Server.SDK.Model.AutomationParameter>();
                foreach (var parameter in automationParameters)
                {
                    var parameterString = JsonConvert.SerializeObject(parameter);
                    var parameterSDK = JsonConvert.DeserializeObject<OpenBots.Server.SDK.Model.AutomationParameter>(parameterString);
                    automationParametersList.Add(parameterSDK);
                }
                apiInstance.ApiVapiVersionAutomationsAutomationIdUpdateParametersPostWithHttpInfo(automationId.ToString(), apiVersion, automationParametersList);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AutomationsApi.ApiVapiVersionAutomationsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
