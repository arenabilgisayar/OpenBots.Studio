using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Server.User;
using OpenBots.Server.SDK.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using IOFile = System.IO.File;

namespace OpenBots.Core.Server.API_Methods
{
    public class AssetMethods
    {
        public static Dictionary<string, string> settings = EnvironmentSettings.GetAgentSettings();
        public static string serverURL = settings["OpenBotsServerUrl"];
        public static AssetsApi apiInstance = new AssetsApi(serverURL);
        public static string apiVersion = "1";

        public static Asset GetAsset(string token, string assetName, string assetType)
        {
            apiInstance.Configuration.AccessToken = token;
            var asset = new Asset();

            try
            {
                var result = apiInstance.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo(assetName, apiVersion, assetType).Result.Data;
                string assetString = JsonConvert.SerializeObject(result);
                asset = JsonConvert.DeserializeObject<Asset>(assetString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo: "
                    + ex.Message);
            }

            return asset;
        }       

        public static void PutAsset(string token, string assetId)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo(assetId, apiVersion).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DownloadFileAsset(RestClient client, Guid? assetID, string directoryPath, string fileName)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Export", Method.GET);
            request.AddUrlSegment("id", assetID.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

            byte[] file = response.RawBytes;
            IOFile.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
        }

        public static void UpdateFileAsset(RestClient client, Asset asset, string filePath)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Update", Method.PUT);
            request.AddUrlSegment("id", asset.Id.ToString());
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("File", filePath.Trim());
            request.AddParameter("Type", "File");
            request.AddParameter("Name", asset.Name);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static void AppendAsset(string token, Guid? assetId, string appendText)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdAppendPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, appendText).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
      
        public static void IncrementAsset(RestClient client, Guid? assetId)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Increment", Method.PUT);
            request.AddUrlSegment("id", assetId.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static void DecrementAsset(RestClient client, Guid? assetId)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Decrement", Method.PUT);
            request.AddUrlSegment("id", assetId.ToString());
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static void AddAsset(RestClient client, Guid? assetId, string value)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Add", Method.PUT);
            request.AddUrlSegment("id", assetId.ToString());
            request.AddQueryParameter("value", value);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }

        public static void SubtractAsset(RestClient client, Guid? assetId, string value)
        {
            var request = new RestRequest("api/v1/Assets/{id}/Subtract", Method.PUT);
            request.AddUrlSegment("id", assetId.ToString());
            request.AddQueryParameter("value", value);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");
        }
    }
}
