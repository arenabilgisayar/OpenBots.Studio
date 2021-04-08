using OpenBots.Commands.Documents.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Library
{
    public class ServiceProxy
    {
        private const string LoginUrlSegment = "api/TokenAuth/Authenticate";
        public static string DefaultUserAgent = "ApiClient";
        public static string DefaultHostUrl = "https://dev-api.documents.openbots.io/"; //TODO: Replace with PROD url.
        public static string DefaultTenantResolveKey = "abp.tenantid";
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(1);
        public string AccessToken { get; set; }

        public string MachineName { get; set; }

        public string ApiKey { get; set; }

        public string TenantId { get; set; }

        public string ServiceUrl { get; set; }

        public string Proxy { get; set; }

        public int Timeout { get; set; }

        public ServiceProxy()
        {
            string endpoint = Environment.GetEnvironmentVariable("TEXTXTRACTOR_ENDPOINT", EnvironmentVariableTarget.User);
            if (endpoint == null || string.IsNullOrEmpty(endpoint))
                ServiceUrl = DefaultHostUrl;
            else
                ServiceUrl = endpoint;

            string proxy = Environment.GetEnvironmentVariable("TEXTXTRACTOR_PROXY", EnvironmentVariableTarget.User);
            if (proxy != null && !string.IsNullOrEmpty(proxy))
                Proxy = proxy;

            Timeout = -1;
        }


        protected virtual O Get<O>(string url, IDictionary<string, string> parameters = null)
        {
            IRestClient client = createClient();
            IRestRequest request = createRequest(url, parameters);

            var response = client.Get<ApiResponse<O>>(request);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            validateRespose<O>(response, url);

            return response.Data.Result;
        }

        protected virtual void Post(string url, object requestBody, IDictionary<string, string> parameters = null)
        {
            IRestClient client = createClient();
            IRestRequest request = createRequest(url, parameters);

            if(requestBody != null)
                request.AddJsonBody(requestBody);

            var response = client.Post(request);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
        }

        protected virtual O Post<O>(string url, object requestBody, IDictionary<string, string> parameters = null)
        {
            IRestClient client = createClient();
            IRestRequest request = createRequest(url, parameters);

            request.AddJsonBody(requestBody);

            var response = client.Post<ApiResponse<O>>(request);
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            validateRespose<O>(response, url);

            return response.Data.Result;
        }

        protected virtual void validateRespose<O>(IRestResponse<ApiResponse<O>> response, string method)
        {
            if (response == null)
                throw new InvalidOperationException($"{method}:Url didnt return a valid response");

            if (response.StatusCode != HttpStatusCode.OK )
                throw new InvalidOperationException($"{method}:Url didnt return a HTTP 200");


            if (response.Data == null)
                throw new InvalidCastException($"{method}:Url didnt return JSON with correct body");

            if (!response.Data.Success)
                throw new InvalidOperationException($"{method}:Url didnt return success from server");

            if (response.Data.UnAuthorizedRequest)
                throw new UnauthorizedAccessException();
        }

        protected virtual IRestRequest createRequest(string url, IDictionary<string, string> parameters = null)
        {
            var request = new RestRequest(url);

            if (!string.IsNullOrEmpty(TenantId))
                request.AddHeader(DefaultTenantResolveKey, TenantId);

            if (!string.IsNullOrEmpty(AccessToken))
                request.AddHeader("Authorization", $"Bearer {AccessToken}");


            if (!string.IsNullOrEmpty(MachineName))
                request.AddHeader("X-MACHINE", MachineName);

            if (!string.IsNullOrEmpty(ApiKey))
                request.AddHeader("X-API-KEY", ApiKey);

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    request.AddQueryParameter(kvp.Key, kvp.Value);
                }
            }


            return request;
        }

        protected virtual IRestClient createClient()
        {
            var client = new RestClient(ServiceUrl);

            client.UserAgent = "Openbots Documents.Activities";

            if (Timeout > -1)
                client.Timeout = Timeout;

            if (!string.IsNullOrEmpty(Proxy))
                client.Proxy = new WebProxy(Proxy);
            return client;
        }

        public virtual void Authenticate(AuthenticationRequest request)
        {

            string username = Environment.GetEnvironmentVariable("TEXTXTRACTOR_USERNAME", EnvironmentVariableTarget.User);
            if (username != null && !string.IsNullOrEmpty(username))
               request.UserNameOrEmailAddress = username;

            string password = Environment.GetEnvironmentVariable("TEXTXTRACTOR_PASSWORD", EnvironmentVariableTarget.User);
            if (password != null && !string.IsNullOrEmpty(password))
                request.Password = password;

            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            AuthenticationResponse response =  Post<AuthenticationResponse>(LoginUrlSegment, request);
            AccessToken = response.AccessToken;
        }


    }
}
