using OpenBots.Server.SDK.Api;
using OpenBots.Server.SDK.Model;
using System;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using FileModel = OpenBots.Core.Server.Models.File;

namespace OpenBots.Core.Server.API_Methods
{
    public class FileMethods
    {
        public static FilesApi apiInstance = new FilesApi(serverURL);

        //public static void DownloadFile(RestClient client, Guid? fileID, string directoryPath, string fileName)
        //{
        //    var request = new RestRequest("api/v1/Files/{id}/download", Method.GET);
        //    request.AddUrlSegment("id", fileID.ToString());
        //    request.RequestFormat = DataFormat.Json;

        //    var response = client.Execute(request);

        //    if (!response.IsSuccessful)
        //        throw new HttpRequestException($"Status Code: {response.StatusCode} - Error Message: {response.ErrorMessage}");

        //    byte[] file = response.RawBytes;
        //    File.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
        //}

        public static FileModel GetFile(string token, Guid? id)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                FileFolderViewModel response = apiInstance.GetFileFolderAsyncWithHttpInfo(id.ToString(), apiVersion).Result.Data;
                var file = new FileModel()
                {
                    Name = response.Name,
                    StoragePath = response.StoragePath,
                    FullStoragePath = response.FullStoragePath,
                    ContentType = response.ContentType
                };
                return file;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling FilesApi.GetFileFolderAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}