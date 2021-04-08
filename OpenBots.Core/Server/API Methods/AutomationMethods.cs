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
    }
}
