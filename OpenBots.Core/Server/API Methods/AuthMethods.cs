using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Server.User;
using OpenBots.Server.SDK.Api;
using OpenBots.Server.SDK.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class AuthMethods
    {
        public static string GetAuthToken()
        {
            string agentId = settings["AgentId"];
            string serverURL = settings["OpenBotsServerUrl"];
            //TODO: server type will be stored in settings??
            //string serverType = "Open Source";
            string serverType = "Cloud";

            if (string.IsNullOrEmpty(agentId))
                throw new Exception("Agent is not connected");

            string username = new RegistryManager().AgentUsername;
            string password = new RegistryManager().AgentPassword;

            if (username == null || password == null)
                throw new Exception("Agent credentials not found in registry");

            if (string.IsNullOrEmpty(serverURL))
                throw new Exception("Server URL not found");

            string token;

            if (serverType == "Open Source")
            {
                var apiInstance = new AuthApi(serverURL);
                var login = new Login(username, password);

                try
                {
                    var result = apiInstance.ApiVapiVersionAuthTokenPostWithHttpInfo(apiVersion, login).Data.ToString();
                    JObject jsonObj = JObject.Parse(result.Replace("[]", "null"));
                    Dictionary<string, string> resultDict = jsonObj.ToObject<Dictionary<string, string>>();
                    token = resultDict["token"].ToString();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Exception when calling AuthApi.ApiVapiVersionAuthTokenPostAsync: " + ex.Message);
                }
            }
            else
            {
                var httpClient = new HttpClient();

                var identityServerResponse = httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest

                {
                    Address = "https://dev.login.openbots.io/connect/token", //TODO: use server url??
                    ClientId = "client",
                    UserName = "nicole.carrero@openbots.ai", //username,
                    Password = "PASSWORDGOESHERE" //password
                }).Result;

                if (identityServerResponse.IsError) throw new Exception(identityServerResponse.Error);

                token = identityServerResponse.AccessToken;
            }
            return token;
        }
    }
}
