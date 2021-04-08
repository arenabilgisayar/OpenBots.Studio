using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextXtractor.Activities.Model;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Submit Document")]
    [Description("Submits a file for Processing by creating a new Task")]
    public class SubmitDocument : BaseActivity, ISubmitFileRequest, ISubmitFileResult
    {

        [Category("Input")]
        [DisplayName("File Path")]
        [RequiredArgument]
        [Description("[Required] Path of the file to be submitted.")]
        public InArgument<string> FilePath { get; set; }

        [Category("Input")]
        [DisplayName("Name of the Task Queue")]
        [Description("Name of the Queue that this task would be created in. If unspecified, it will be defauted to the 'Common' queue.")]
        public InArgument<string> QueueName { get; set; }

        [Category("Input")]
        [DisplayName("Name / Title of the Task")]
        [RequiredArgument]
        [Description("Name or Title of the task to be created.")]
        public InArgument<string> Name { get; set; }

        [Category("Input")]
        [DisplayName("Description")]
        [Description("Description of the task for Reviewers or other downstream processing.")]
        public InArgument<string> Description { get; set; }


        [Category("Input")]
        [DisplayName("Case Number")]
        [Description("A case number for reference")]
        public InArgument<string> CaseNumber { get; set; }

        [Category("Input")]
        [DisplayName("Case Type")]
        [Description("A case type for reference")]
        public InArgument<string> CaseType { get; set; }


        [Category("Input")]
        [DisplayName("Assign To User")]
        [Description("Email address of the user to assign the task")]
        public InArgument<string> AssignedTo { get; set; }


        [Category("Input")]
        [DisplayName("Due Date of the Task")]
        [Description("Due Date for the Task")]
        public InArgument<DateTime> DueDate { get; set; }


        [Category("Output")]
        [DisplayName("TaskID")]
        [Description("A identification number for the task created that can be used for subsequent calls / queries.")]
        public OutArgument<Guid> TaskID { get; set; }

        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Status of the task just after submitting the document. Expect 'Created' or 'InProgress'")]
        public OutArgument<string> Status { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
            //Trace.WriteLine("TRACE: Starting Submit");
            //Debug.WriteLine("DEBUG: Starting Submit");
            //ILog log = LogManager.GetLogger(this.GetType());
            //log.Debug("Starting Submit Document");



            string fileToProcess = FilePath.Get(context);
            string taskQueueName = QueueName.Get(context);
            Guid taskQueueId;
            string name = Name.Get(context);
            string description = Description.Get(context);
            string caseNumber = CaseNumber.Get(context);
            string caseType = CaseType.Get(context);
            string assignedTo = AssignedTo.Get(context);
            string dueOn = DueDate.Get(context).ToString();

            //Trace.WriteLine($"Processing File {fileToProcess}");

            if (!File.Exists(fileToProcess))
                throw new FileNotFoundException($"ERROR: File not found.{fileToProcess}");


            DocumentProcessingService dps = CreateAuthenticatedService(context);

            //// Submits the document to the server for processing
            var docResponse = dps.Submit(fileToProcess, taskQueueName, name, description, caseNumber, caseType, assignedTo, dueOn);


            //// Retrieve the ID of the submitted document for further querying and retrieval.
            var humanTaskId = new Guid( docResponse.humanTaskID); // new Guid("00ea6030-40ce-4495-8ef2-418eb0845e62");// 

            TaskID.Set(context, humanTaskId);

            string status = dps.GetStatus(humanTaskId);

            Status.Set(context, status);

        }


     
    }
}
