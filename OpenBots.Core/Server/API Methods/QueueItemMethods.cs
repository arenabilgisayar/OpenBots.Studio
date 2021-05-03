using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Core.Server.API_Methods
{
    public class QueueItemMethods
    {
        public static QueueItemsApi apiInstance = new QueueItemsApi(serverURL);
        public static QueueItemAttachmentsApi attachmentApiInstance = new QueueItemAttachmentsApi(serverURL);

        public static QueueItem GetQueueItemById(string token, Guid? id)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.GetQueueItemAsyncWithHttpInfo(id.ToString(), apiVersion).Result.Data;
                string queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItem>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.GetQueueItemAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static QueueItem GetQueueItemByLockTransactionKey(string token, string transactionKey)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                string filter = $"LockTransactionKey eq guid'{transactionKey}'";
                var result = apiInstance.ApiVapiVersionQueueItemsGetAsyncWithHttpInfo(apiVersion, filter).Result.Data.Items.FirstOrDefault();
                string queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItem>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void EnqueueQueueItem(string token, QueueItem queueItem)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var queueItemString = JsonConvert.SerializeObject(queueItem);
                var queueItemSDK = JsonConvert.DeserializeObject<OpenBots.Server.SDK.Model.QueueItem>(queueItemString);
                apiInstance.ApiVapiVersionQueueItemsEnqueuePostAsyncWithHttpInfo(apiVersion, queueItemSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsEnqueuePostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void AttachFiles(string token, Guid? queueItemId, List<string> attachments)
        {
            attachmentApiInstance.Configuration.AccessToken = token;

            try
            {
                List<FileStream> attachmentsList = new List<FileStream>();
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        FileStream _file = new FileStream(attachment, FileMode.Open, FileAccess.Read);
                        attachmentsList.Add(_file);
                    }
                }

                attachmentApiInstance.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsPostAsyncWithHttpInfo(queueItemId.ToString(), apiVersion, attachmentsList).Wait();

                foreach (var file in attachmentsList)
                {
                    file.Flush();
                    file.Dispose();
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static QueueItem DequeueQueueItem(string token, string agentId, Guid? queueId)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.ApiVapiVersionQueueItemsDequeueGetAsyncWithHttpInfo(apiVersion, agentId, queueId.ToString()).Result.Data;
                //may have to map here since result returns a view model
                var queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItem>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsDequeueGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void CommitQueueItem(string token, Guid transactionKey)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionQueueItemsCommitPutAsyncWithHttpInfo(apiVersion, transactionKey.ToString()).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsCommitPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void RollbackQueueItem(string token, Guid transactionKey, string code, string error, bool isFatal)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionQueueItemsRollbackPutAsyncWithHttpInfo(apiVersion, transactionKey.ToString(), code, error, isFatal).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsRollbackPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void ExtendQueueItem(string token, Guid transactionKey)
        {
            apiInstance.Configuration.AccessToken = token;

            try
            {
                apiInstance.ApiVapiVersionQueueItemsExtendPutAsyncWithHttpInfo(apiVersion, transactionKey.ToString()).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsExtendPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static List<QueueItemAttachment> GetAttachments(string token, Guid? queueItemId)
        {
            attachmentApiInstance.Configuration.AccessToken = token;

            try
            {
                var attachments = attachmentApiInstance.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsGetAsyncWithHttpInfo(queueItemId.ToString(), apiVersion).Result.Data.Items;
                var listString = JsonConvert.SerializeObject(attachments);
                var attachmentsList = JsonConvert.DeserializeObject<List<QueueItemAttachment>>(listString);

                return attachmentsList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.GetQueueItemAttachmentsAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DownloadFile(string token, QueueItemAttachment attachment, string directoryPath)
        {
            attachmentApiInstance.Configuration.AccessToken = token;

            try
            {
                var file = FileMethods.GetFile(token, attachment.FileId);
                MemoryStream response = attachmentApiInstance.ExportQueueItemAttachmentAsyncWithHttpInfo(attachment.Id.ToString(), apiVersion, attachment.QueueItemId.ToString()).Result.Data;
                byte[] fileArray = response.ToArray();
                System.IO.File.WriteAllBytes(Path.Combine(directoryPath, file.Name), fileArray);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.ExportQueueItemAttachmentAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
