using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.Collections.Generic;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class ServerEmailMethods
    {
        public static EmailsApi apiInstance = new EmailsApi(serverURL);

        public static void SendServerEmail(string token, EmailMessage emailMessage, List<string> attachments, string accountName)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                List<FileStream> attachmentsList = new List<FileStream>();
                if (attachments != null)
                {
                    
                    foreach (var attachment in attachments)
                    {
                        FileStream _file = new FileStream(attachment, FileMode.Open, FileAccess.Read);
                        attachmentsList.Add(_file);
                    }
                }

                var emailMessageJson = JsonConvert.SerializeObject(emailMessage);
                apiInstance.ApiVapiVersionEmailsSendPostAsyncWithHttpInfo(apiVersion, emailMessageJson, attachmentsList, accountName).Wait();

                foreach (var file in attachmentsList)
                {
                    file.Flush();
                    file.Dispose();
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling EmailsApi.ApiVapiVersionEmailsSendPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
