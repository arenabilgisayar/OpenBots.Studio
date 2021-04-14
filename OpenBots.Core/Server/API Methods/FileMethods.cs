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