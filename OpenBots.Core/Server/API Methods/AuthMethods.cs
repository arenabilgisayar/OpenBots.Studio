using Newtonsoft.Json.Linq;
using OpenBots.Core.Server.User;
using OpenBots.Server.SDK.Api;
using OpenBots.Server.SDK.Model;
using System;
using System.Collections.Generic;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class AuthMethods
    {
        public static string GetAuthToken()
        {
            string agentId = settings["AgentId"];
            string serverURL = settings["OpenBotsServerUrl"];

            if (string.IsNullOrEmpty(agentId))
                throw new Exception("Agent is not connected");

            string username = new RegistryManager().AgentUsername;
            string password = new RegistryManager().AgentPassword;

            if (username == null || password == null)
                throw new Exception("Agent credentials not found in registry");

            if (string.IsNullOrEmpty(serverURL))
                throw new Exception("Server URL not found");

            var apiInstance = new AuthApi(serverURL);
            var login = new Login(username, password);
            string token;

            try
            {
                var result = apiInstance.ApiVapiVersionAuthTokenPostAsyncWithHttpInfo(apiVersion, login).Result.Data.ToString();
                JObject jsonObj = JObject.Parse(result.Replace("[]", "null"));
                Dictionary<string, string> resultDict = jsonObj.ToObject<Dictionary<string, string>>();
                token = resultDict["token"].ToString();
                return token;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AuthApi.ApiVapiVersionAuthTokenPostAsync: " + ex.Message);
            }
        }
    }
}
