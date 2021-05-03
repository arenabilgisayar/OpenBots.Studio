using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using IOFile = System.IO.File;

namespace OpenBots.Core.Server.API_Methods
{
    public class AssetMethods
    {
        public static AssetsApi apiInstance = new AssetsApi(serverURL);

        public static Asset GetAsset(string token, string assetName, string assetType)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo(assetName, apiVersion, assetType).Result.Data;
                string assetString = JsonConvert.SerializeObject(result);
                var asset = JsonConvert.DeserializeObject<Asset>(assetString);
                return asset;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }       

        public static void PutAsset(string token, Asset asset)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var assetString = JsonConvert.SerializeObject(asset);
                var assetSDK = JsonConvert.DeserializeObject<OpenBots.Server.SDK.Model.Asset>(assetString);
                apiInstance.ApiVapiVersionAssetsIdPutAsyncWithHttpInfo(asset.Id.ToString(), apiVersion, assetSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsIdPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DownloadFileAsset(string token, Guid? assetId, string directoryPath, string fileName)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                MemoryStream response = apiInstance.ExportAssetAsyncWithHttpInfo(assetId.ToString(), apiVersion).Result.Data;
                byte[] file = response.ToArray();
                IOFile.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ExportAssetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void UpdateFileAsset(string token, Asset asset, string filePath)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    apiInstance.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo(asset.Id.ToString(), apiVersion, asset.Name, asset.Type, null, 0, null, asset.FileId.Value, _file).Wait();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo: "
                    + ex.Message);
            }
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
      
        public static void IncrementAsset(string token, Guid? assetId)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdIncrementPutAsyncWithHttpInfo(assetId.ToString(), apiVersion).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdIncrementPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DecrementAsset(string token, Guid? assetId)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdDecrementPutAsyncWithHttpInfo(assetId.ToString(), apiVersion).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdDecrementPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void AddAsset(string token, Guid? assetId, int value)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdAddPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, value).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdAddPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void SubtractAsset(string token, Guid? assetId, int value)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionAssetsIdSubtractPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, value).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdSubtractPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
