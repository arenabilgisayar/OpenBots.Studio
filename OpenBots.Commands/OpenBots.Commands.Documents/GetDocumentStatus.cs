using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Get Status")]
    [Description("Retrieves the current status of the document being processed. Also allows to wait for the completion.")]
    public class GetDocumentStatus : BaseActivity, IGetStatusRequest, IGetStatusResult
    {

        [Category("Input")]
        [DisplayName("TaskID")]
        [Description("Task Identifier that was provided while submiting the document.")]
        public InArgument<Guid> TaskID { get; set; }

        [Category("Input")]
        [DisplayName("Await Completion")]
        [DefaultValue(false)]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. Awaiting queries the service for status every 10 seconds until completed.")]
        public InArgument<bool> AwaitCompletion { get; set; }

        [Category("Input")]
        [DisplayName("Timeout (in seconds)")]
        [DefaultValue(120)]
        [Description("Timeout if awaiting for document processing to be completed.")]
        public InArgument<int> TimeoutInSeconds { get; set; }


        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Status of the task/document submitted for processing. Expect 'Created' or 'InProgress'")]
        public OutArgument<string> Status { get; set; }

        [Category("Output")]
        [DisplayName("Is Document Completed")]
        [Description("True if the document has finished Processing")]
        public OutArgument<bool> IsDocumentCompleted { get; set; }

        [Category("Output")]
        [DisplayName("Has Error")]
        [Description("Document Processing has errors and couldnt complete.")]
        public OutArgument<bool> HasError { get; set; }

        [Category("Output")]
        [DisplayName("Is Currently Processing")]
        [Description("Document is currently being processed")]
        public OutArgument<bool> IsCurrentlyProcessing { get; set; }


        [Category("Output")]
        [DisplayName("Is Successful")]
        [Description("Is Document Processing Completed Successfully and read for results data to be read")]
        public OutArgument<bool> IsSuccessful { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            DocumentProcessingService ds = CreateAuthenticatedService(context);

            Guid taskid = TaskID.Get(context);
            string status = ds.GetStatus(taskid);

            if(AwaitCompletion.Get(context))
            {
                int timeout = TimeoutInSeconds.Get(context);
                status = ds.AwaitProcessing(taskid, timeout);
            }

            Status.Set(context, status);

            if (status == "Processed")
            {
                IsDocumentCompleted.Set(context, true);
                HasError.Set(context, false);
                IsCurrentlyProcessing.Set(context, false);
                IsSuccessful.Set(context, true);
            }
            else
            {
                IsSuccessful.Set(context, false);
                IsDocumentCompleted.Set(context, false);
            }

            if (status == "InProgress")
            {
                HasError.Set(context, false);
                IsCurrentlyProcessing.Set(context, true);
                IsSuccessful.Set(context, false);
            }

            if (status == "CompletedWithError")
            {
                IsDocumentCompleted.Set(context, true);
                HasError.Set(context, true);
                IsCurrentlyProcessing.Set(context, false);
            }

            

        }
    }




}
