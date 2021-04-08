using Newtonsoft.Json;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextXtractor.Activities.Model;

namespace OpenBots.Commands.Documents
{

    [Category("Openbots Documents")]
    [DisplayName("Save Document Results")]
    [Description("Saves the processing results in a file system folder.")]
    public class SaveDocumentResults : BaseActivity, ISaveRequest, ISaveResult
    {

        [Category("Input")]
        [DisplayName("TaskID")]
        [RequiredArgument]
        [Description("Task Identifier that was provided while submiting the document.")]
        public InArgument<Guid> TaskID { get; set; }

        [Category("Input")]
        [DisplayName("Await Completion")]
        [DefaultValue(false)]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. Awaiting queries the service for status every 10 seconds until completed.")]
        public InArgument<bool> AwaitCompletion { get; set; }

        [Category("Input")]
        [DisplayName("Save Page Images")]
        [DefaultValue(false)]
        [Description("Allows the service to download Images of each page.")]
        public InArgument<bool> SavePageImages { get; set; }

        [Category("Input")]
        [DisplayName("Save Page Text")]
        [DefaultValue(false)]
        [Description("Allows the service to download Text of each page.")]
        public InArgument<bool> SavePageText { get; set; }

        [Category("Input")]
        [DisplayName("Timeout (in seconds)")]
        [DefaultValue(120)]
        [Description("Timeout if awaiting for document processing to be completed.")]
        public InArgument<int> TimeoutInSeconds { get; set; }

        [Category("Input")]
        [DisplayName("Output Folder")]
        [Description("Folder in which the resulting text and documents are saved.")]
        [RequiredArgument]
        public InArgument<string> OutputFolder { get; set; }

        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Returns the status of the processing.")]
        public OutArgument<string> Status { get; set; }

        [Category("Output")]
        [DisplayName("Is Document Process Completed")]
        [Description("Returns if the document processing was completed.")]
        public OutArgument<bool> IsCompleted { get; set; }

        [Category("Output")]
        [DisplayName("Has Failed or Has Errors")]
        [Description("Returns if the document processing has errors or has failed.")]
        public OutArgument<bool> HasFailedOrError { get; set; }

        [Category("Output")]
        [DisplayName("OutputAsJSON")]
        [Description("Returns the documents extracted as an output for this task as a JSON String")]
        public OutArgument<string> OutputAsJSON { get; set; }

        [Category("Output")]
        [DisplayName("OutputAsTable")]
        [Description("Returns the documents extracted as an output for this task as a DataTable. Columns are DocumentNumber (int), Schema (string), PageNumbers (string), Folder (string), DocumentId (string), Confidence (double)")]
        public OutArgument<DataTable> OutputAsTable { get; set; }


        [Category("Output")]
        [DisplayName("DataAsTable")]
        [Description("Appends the data extracted as an output for this task in a DataTable. Columns are TaskId (string), DocumentId (string), DocumentNumber (int), Schema (string), PageNumbers (string), <fields of from all schemas found>. Simply use 'Write CSV' to save these results.")]
        public InOutArgument<DataTable> DataAsTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {

            DocumentProcessingService service = CreateAuthenticatedService(context);

            var humanTaskId = TaskID.Get(context);
            bool awaitCompletion = AwaitCompletion.Get(context);
            int timeout = TimeoutInSeconds.Get(context);
            bool savePageText = SavePageText.Get(context);
            bool savePageImages = SavePageImages.Get(context);
            bool hasFailed = true;
            bool isCompleted = false;
            string status = "";
            string outputFolder = OutputFolder.Get(context);
            DataTable dataTable = DataAsTable.Get(context);

            DocumentInfo docInfo =  service.SaveDocumentLocally(humanTaskId, awaitCompletion, timeout, savePageText, savePageImages, outputFolder,ref  dataTable, out status, out hasFailed, out isCompleted);

            OutputAsJSON.Set(context, docInfo.SerializeJSON());
            OutputAsTable.Set(context, docInfo.CreateDataTable());
            DataAsTable.Set(context, dataTable);


            Status.Set(context, status);
            IsCompleted.Set(context, isCompleted);
            HasFailedOrError.Set(context, hasFailed);
        }


    }


}
