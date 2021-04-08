using Newtonsoft.Json;
using OpenBots.Commands.Documents.Exceptions;
using OpenBots.Commands.Documents.Models;
using RestSharp;
//using SimpleJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Library
{
    public class DocumentProcessingService : ServiceProxy
    {
        private const string GetStatusUrlSegment = "api/services/app/DocumentProcessingEngineService/GetStatus";
        private const string GetQueuesUrlSegment = "api/services/app/DocumentProcessingEngineService/GetQueues?MaxResultCount=100";
        private const string SubmitUrlSegment = "api/services/app/DocumentProcessingEngineService/SubmitDocumentsWithDetails";

        private const string GetDocumentsUrlSegment = "api/services/app/DocumentProcessingEngineService/GetDocuments";
        private const string GetDocumentDataUrlSegment = "api/services/app/DocumentProcessingEngineService/GetDocumentData";
        private const string GetPageImageUrlSegment = "api/services/app/DocumentProcessingEngineService/GetPageImage";
        private const string GetPageTextUrlSegment = "api/services/app/DocumentProcessingEngineService/GetPageText";

        private const string ChangeStatusUrlSegment = "api/services/app/DocumentProcessingEngineService/ChangeStatus";
        private const string MarkDocumentAsVerifiedUrlSegment = "api/services/app/DocumentProcessingEngineService/MarkDocumentAsVerified";

        private const string DequeueUrlSegment = "/api/services/app/AppQueueItems/Dequeue";
        private const string CommitUrlSegment = "/api/services/app/AppQueueItems/Commit";
        private const string RollbackUrlSegment = "/api/services/app/AppQueueItems/Rollback";

        private const string CreateTaskUrlSegment = "/api/services/app/HumanTasks/CreateOrEdit";


        public Guid CreateTask(CreateTaskRequest task)
        {
           return Post<Guid>(CreateTaskUrlSegment, task);
        }

        public string GetStatus(Guid taskId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            return Get<string>(GetStatusUrlSegment, parameters);

        }

        public string MarkDocumentAsVerified(Guid taskId, Guid documentId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            parameters.Add("documentId", taskId.ToString());

            return Post<string>(MarkDocumentAsVerifiedUrlSegment, null, parameters);

        }

        public void ChangeStatus(Guid taskId, TaskStatusTypes taskStatus)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            parameters.Add("newStatus", taskStatus.ToString());

            Post(ChangeStatusUrlSegment, null, parameters);

        }

        public List<ExtractedDocumentView> GetDocuments(Guid taskId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            return Get<List<ExtractedDocumentView>>(GetDocumentsUrlSegment, parameters);
        }

        public DocumentContentView GetDocumentData(Guid taskId, Guid documentId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            parameters.Add("documentId", documentId.ToString());
            return Get<DocumentContentView>(GetDocumentDataUrlSegment, parameters);
        }


        public string GetPageText(Guid taskId, Guid documentId, int page)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            parameters.Add("documentId", documentId.ToString());
            parameters.Add("page", page.ToString());
            return Get<string>(GetPageTextUrlSegment, parameters);
        }

        public Image GetPageImage(Guid taskId, Guid documentId, int page)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("humanTaskId", taskId.ToString());
            parameters.Add("documentId", documentId.ToString());
            parameters.Add("page", page.ToString());
            var pageDetails = Get<string>(GetPageImageUrlSegment, parameters);

            var imagedata = Convert.FromBase64String(pageDetails);

            Bitmap bmp;
            Image image;
            using (var ms = new MemoryStream(imagedata))
            {
                image = Image.FromStream(ms);
                //bmp = new Bitmap(ms);
            }

            return image;
        }



        public Dictionary<string, string> GetQueues()
        {
            return Get<Dictionary<string, string>>(GetQueuesUrlSegment);
            
        }


        public string AwaitProcessing(Guid humanTaskId, int timeoutInSeconds = 120)
        {
            bool continueTrying = true;
            DateTime startTime = DateTime.Now;
            string status = "";
            while (continueTrying)
            {
                status = GetStatus(humanTaskId);

                if (status == "Processed" || status == "CompletedWithError" || DateTime.Now.Subtract(startTime).TotalSeconds >= timeoutInSeconds)
                {
                    continueTrying = false;
                    break;
                }
                else
                {
                    Task.Delay(TimeSpan.FromSeconds(20)).Wait();
                }
            }

            return status;

        }

        public SubmitDocumentResponse Submit(
            string filePath,
            string taskQueueName = "",
            string name = "",
            string description = "",
            string caseNumber = "",
            string caseType = "",
            string assignedTo = "",
            string dueOn = "")
        {
            if (string.IsNullOrEmpty(taskQueueName))
            {
                taskQueueName = "Common";
            }


            Dictionary<string, string> allQueues = null;
            allQueues = GetQueues();

            if (allQueues == null)
                throw new CannotRetrieveQueuesException();

            if (!allQueues.Where(q => q.Value == taskQueueName).Any())
                throw new UnableToFindQueueException($"Unable to find queue {taskQueueName} ");

            var queueItem = allQueues.Where(kp => kp.Value == taskQueueName).FirstOrDefault();
            Guid taskQueueId = new Guid(queueItem.Key);

            //// Submits the document to the server for processing
            var docResponse = SubmitDocument(filePath, taskQueueId, name, description, caseNumber, caseType, assignedTo, dueOn);

            if (docResponse == null)
            {
                throw new CannotSubmitDocumentToServiceException();
            }

            if (docResponse.humanTaskID == null || string.IsNullOrEmpty(docResponse.humanTaskID))
            {
                throw new InvalidDataException("ERROR: Service did not return any TaskID.");
            }

            return docResponse;
        }

        public SubmitDocumentResponse SubmitDocument(
                string filePath,
                Guid? taskQueueId = null,
                string name = "",
                string description = "",
                string caseNumber = "",
                string caseType = "",
                string assignedTo = "",
                string dueOn = "")
        {

            CreateTaskRequest taskR = new CreateTaskRequest();

            if (taskQueueId != null && taskQueueId.HasValue)
                taskR.TaskQueueId = taskQueueId.Value;

            if (!string.IsNullOrEmpty(name))
                taskR.Name = name;

            if (!string.IsNullOrEmpty(description))
                taskR.Description = description;

            if (!string.IsNullOrEmpty(caseNumber))
                taskR.CaseNumber = caseNumber;

            if (!string.IsNullOrEmpty(caseType))
                taskR.CaseType = caseType;

            if (!string.IsNullOrEmpty(assignedTo))
                taskR.AssignedTo = assignedTo;

            if (!string.IsNullOrEmpty(dueOn))
                taskR.DueOn = dueOn;

            taskR.Status = "Creating";

            Guid docId = CreateTask(taskR);

            string contentType = MimeTypeMap.GetMimeType(Path.GetExtension(filePath));

            var client = createClient();
            var request = createRequest(SubmitUrlSegment);

            //request.AddHeader("Content-Type", "multipart/form-data");
            //request.AlwaysMultipartFormData = true;
            request.AddQueryParameter("humanTaskId", docId.ToString());
            request.AddFile("files", filePath, contentType);

            var res = client.Post<ApiResponse<SubmitDocumentResponse>>(request);

            validateRespose<SubmitDocumentResponse>(res, "Submit");

            return res.Data.Result;
            
       }

        public DocumentInfo SaveDocumentLocally(Guid humanTaskId, bool awaitCompletion, int timeout, bool savePageText, bool savePageImages, string outputFolder, ref DataTable dataTable, out string status, out bool hasFailed, out bool isCompleted)
        {
            hasFailed = true;
            isCompleted = false;
            string saveJson = "";
            DocumentInfo docInfo = new DocumentInfo();
            docInfo.TaskId = humanTaskId.ToString();
            List<string> datacolumns = new List<string>();


            // Prepare DataTable if it doesnt have default rows
            if (dataTable == null)
            {
                dataTable = new DataTable("ExtractedData");
            }
            if (dataTable != null)
            {
                datacolumns = dataTable.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName).ToList();

                if (!datacolumns.Contains("FileName"))
                    dataTable.Columns.Add(new DataColumn("FileName", typeof(string)));

                //TaskId	DocumentId	Pages	Schema	IsUnstructured
                if (!datacolumns.Contains("TaskId"))
                    dataTable.Columns.Add(new DataColumn("TaskId", typeof(string)));

                if (!datacolumns.Contains("DocumentId"))
                    dataTable.Columns.Add(new DataColumn("DocumentId", typeof(string)));

                if (!datacolumns.Contains("Pages"))
                    dataTable.Columns.Add(new DataColumn("Pages", typeof(string)));

                if (!datacolumns.Contains("Schema"))
                    dataTable.Columns.Add(new DataColumn("Schema", typeof(string)));

                if (!datacolumns.Contains("IsUnstructured"))
                    dataTable.Columns.Add(new DataColumn("IsUnstructured", typeof(bool)));



                datacolumns = dataTable.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName).ToList();

            }

            status = GetStatus(humanTaskId);
            

            if (awaitCompletion)
            {
                // Await for the document to be processed. This is for synchronous wait.
                status = AwaitProcessing(humanTaskId, timeout);
            }
            // Incase you dont want to wait for the processing, you can call GetStatus and check for status to be 'Processed'
            //string status  = service.GetStatus(humanTaskId).Result;
            if (string.IsNullOrEmpty(status))
                Trace.WriteLine($"ERROR: Something went wrong. Status of a Task cannot be null.");

            if (status == "Created" || status == "Creating" || status == "InProgress" || status == "CompletedWithError" || status == "Error")
            {
                Trace.WriteLine($"ERROR: Document is not processed yet. Most likely we timed out.");
                return docInfo;
            }

            // Once the document is processed, GetDocuments will retierve the Extracted Documents

            var docs = GetDocuments(humanTaskId);
            if (docs == null)
            {
                Trace.WriteLine($"ERROR: No documents extracted.");
                return docInfo;
            }

            if (string.IsNullOrEmpty(outputFolder))
            {
                throw new ArgumentNullException($"OutputFolder Directory not found");
            }


            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            if (!Directory.Exists(outputFolder))
            {
                throw new DirectoryNotFoundException($"Directory {outputFolder} not found");
            }

            DirectoryInfo targetFolder = new DirectoryInfo(outputFolder);
           

            int dociterator = 1;
            foreach (var doc in docs.OrderBy(d => d.Order))
            {
                DocumentContentView docData = null;

                docData = GetDocumentData(humanTaskId, doc.DocumentId.Value);
                Task.Delay(TimeSpan.FromSeconds(1)).Wait();

                DocumentView docSave = new DocumentView();
                docSave.TaskID = humanTaskId;
                docSave.Header = doc;
                docSave.Content = docData;


                string schemaName = doc.Schema.Replace(Path.DirectorySeparatorChar, '_').Replace("/", "_");

                DirectoryInfo docFolder = targetFolder.CreateSubdirectory($"{dociterator} ({schemaName})");

                docInfo.Add(dociterator, doc.Schema, doc.PageRangeLabel, docFolder.Name, doc.DocumentId.ToString());

                DataRow currentRow = null;
                if (dataTable != null)
                {
                    currentRow = dataTable.NewRow();
                    //  if (!datacolumns.Contains("FileName"))
                    currentRow["FileName"] = doc.Name;
                    currentRow["TaskId"] = humanTaskId.ToString();
                    currentRow["DocumentId"] = doc.DocumentId.ToString();

                    currentRow["Pages"] = doc.PageRangeLabel;
                    currentRow["Schema"] = doc.Schema;
                    currentRow["IsUnstructured"] = true;
                }


                if (!schemaName.ToLowerInvariant().Equals("unstructured", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(docData.Content))
                    {
                        try
                        {
                            dynamic json = JsonConvert.DeserializeObject(docData.Content);
                            string jsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
                            string dataFile = Path.Combine(docFolder.FullName, "data.json");
                            File.WriteAllText(dataFile, jsonString);
                            docSave.Content.Content = "";

                            if (dataTable != null)
                            {

                                var exData =  ExtractedContentField.Parse(jsonString);
                                if (exData != null)
                                {
                                    foreach (string key in exData.Keys)
                                    {
                                        if(!datacolumns.Contains(key))
                                        {
                                            dataTable.Columns.Add(new DataColumn(key, typeof(string)));
                                        }
                                    }
                                }

                                datacolumns = dataTable.Columns.Cast<DataColumn>()
                               .Select(x => x.ColumnName).ToList();

                                if (dataTable != null)
                                {
                                    currentRow = dataTable.NewRow();
                                    currentRow["FileName"] = doc.Name;
                                    currentRow["TaskId"] = humanTaskId.ToString();
                                    currentRow["DocumentId"] = doc.DocumentId.ToString();

                                    currentRow["Pages"] = doc.PageRangeLabel;
                                    currentRow["Schema"] = doc.Schema;
                                    currentRow["IsUnstructured"] = false;
                                    foreach (string key in exData.Keys)
                                    {
                                        currentRow[key] = exData[key].value;
                                    }
                                }

                            }

                        }
                        catch { }
                    }
                }


                if (savePageText || savePageImages)
                {
                    foreach (var page in doc.Pages)
                    {

                        if (savePageImages)
                        {
                            Image pageImage = null;
                            try
                            {
                                pageImage = GetPageImage(humanTaskId, doc.DocumentId.Value, page.File.Value);
                                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                            }
                            catch { }

                            if (pageImage != null)
                            {
                                string imagePath = Path.Combine(docFolder.FullName, $"Page_{page.File.Value}.jpg");
                                var i2 = new Bitmap(pageImage);
                                i2.Save(imagePath, ImageFormat.Jpeg);

                            }
                        }

                        if (savePageText)
                        {
                            string pageText = "";

                            try
                            {
                                pageText = GetPageText(humanTaskId, doc.DocumentId.Value, page.File.Value);
                                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                            }
                            catch { }

                            if (!string.IsNullOrEmpty(pageText))
                            {
                                File.WriteAllText(Path.Combine(docFolder.FullName, $"{page.File.Value}.txt"), pageText);
                                docSave.Content.Content = "";
                            }
                        }
                    }
                }
                if (docSave != null)
                {
                    var docJsonContent = JsonConvert.SerializeObject(docSave, Formatting.Indented);
                    string docFile = Path.Combine(docFolder.FullName, "document.json");
                    File.WriteAllText(docFile, docJsonContent);
                }

                if (currentRow != null)
                    dataTable.Rows.Add(currentRow);

                dociterator++;
            }

            if (docInfo != null)
            {
                string docInfoFile = Path.Combine(targetFolder.FullName, "documents.json");
                var docInfoContent = docInfo.SerializeJSON();
                File.WriteAllText(docInfoFile, docInfoContent);
                saveJson = docInfoContent;
            }

        

            hasFailed = false;
            isCompleted = true;

            return docInfo;
        }


    }
}
