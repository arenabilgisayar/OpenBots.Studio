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

        public static void SendServerEmail(string token, EmailMessage emailMessage, string attachments, string accountName)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                List<FileStream> attachmentsList = new List<FileStream>();
                if (!string.IsNullOrEmpty(attachments))
                {
                    var splitAttachments = attachments.Split(';');
                    foreach (var vAttachment in splitAttachments)
                    {
                        FileStream _file = new FileStream(vAttachment, FileMode.Open, FileAccess.Read);
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

        public static List<EmailAddress> GetEmailList(string recipients)
        {
            var emailList = new List<EmailAddress>();

            if (!string.IsNullOrEmpty(recipients))
            {
                var splitRecipients = recipients.Split(';');
                foreach (var recipient in splitRecipients)
                {
                    var email = new EmailAddress()
                    {
                        Name = recipient,
                        Address = recipient
                    };
                    emailList.Add(email);
                }
            }

            return emailList;
        }
    }
}
