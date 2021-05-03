using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using static OpenBots.Core.Server.User.EnvironmentSettings;


namespace OpenBots.Core.Server.API_Methods
{
    public class CredentialMethods
    {
        public static CredentialsApi apiInstance = new CredentialsApi(serverURL);

        public static Credential GetCredential(string token, string name)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.ApiVapiVersionCredentialsGetCredentialByNameCredentialNameGetAsyncWithHttpInfo(name, apiVersion).Result.Data;
                //string filter = $"name eq '{name}'";
                //var result = apiInstance.ApiVapiVersionCredentialsGetAsyncWithHttpInfo(apiVersion, filter).Result.Data.Items.FirstOrDefault();
                var resultJson = JsonConvert.SerializeObject(result);
                var credential = JsonConvert.DeserializeObject<Credential>(resultJson);

                return credential;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling CredentialsApi.ApiVapiVersionCredentialsGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void PutCredential(string token, Credential credential)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var credentialJson = JsonConvert.SerializeObject(credential);
                var credentialSDK = JsonConvert.DeserializeObject<OpenBots.Server.SDK.Model.Credential>(credentialJson);
                apiInstance.ApiVapiVersionCredentialsIdPutAsyncWithHttpInfo(credential.Id.ToString(), apiVersion, credentialSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling CredentialsApi.ApiVapiVersionCredentialsIdPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
