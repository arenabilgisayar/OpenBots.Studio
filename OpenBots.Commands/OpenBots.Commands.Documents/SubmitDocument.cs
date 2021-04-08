using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Submit Document")]
    [Description("Submits a file for Processing by creating a new Task")]
    public class SubmitDocument : BaseActivity, ISubmitFileRequest, ISubmitFileResult
    {

        [Category("Input")]
        [DisplayName("File Path")]
        [Required]
        [Description("[Required] Path of the file to be submitted.")]
        public string v_FilePath { get; set; }

        [Category("Input")]
        [DisplayName("Name of the Task Queue")]
        [Description("Name of the Queue that this task would be created in. If unspecified, it will be defauted to the 'Common' queue.")]
        public string v_QueueName { get; set; }

        [Category("Input")]
        [DisplayName("Name / Title of the Task")]
        [Required]
        [Description("Name or Title of the task to be created.")]
        public string v_Name { get; set; }

        [Category("Input")]
        [DisplayName("Description")]
        [Description("Description of the task for Reviewers or other downstream processing.")]
        public string v_Description { get; set; }


        [Category("Input")]
        [DisplayName("Case Number")]
        [Description("A case number for reference")]
        public string v_CaseNumber { get; set; }

        [Category("Input")]
        [DisplayName("Case Type")]
        [Description("A case type for reference")]
        public string v_CaseType { get; set; }


        [Category("Input")]
        [DisplayName("Assign To User")]
        [Description("Email address of the user to assign the task")]
        public string v_AssignedTo { get; set; }


        [Category("Input")]
        [DisplayName("Due Date of the Task")]
        [Description("Due Date for the Task")]
        public string v_DueDate { get; set; } //DateTime


        [Category("Output")]
        [DisplayName("TaskID")]
        [Description("A identification number for the task created that can be used for subsequent calls / queries.")]
        public string v_TaskID { get; set; } //Guid

        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Status of the task just after submitting the document. Expect 'Created' or 'InProgress'")]
        public string v_Status { get; set; }


        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            //Trace.WriteLine("TRACE: Starting Submit");
            //Debug.WriteLine("DEBUG: Starting Submit");
            //ILog log = LogManager.GetLogger(this.GetType());
            //log.Debug("Starting Submit Document");

            string vFileToProcess = v_FilePath.ConvertUserVariableToString(engine);
            string vTaskQueueName = v_QueueName.ConvertUserVariableToString(engine);
            string vName = v_Name.ConvertUserVariableToString(engine);
            string vDescription = v_Description.ConvertUserVariableToString(engine);
            string vCaseNumber = v_CaseNumber.ConvertUserVariableToString(engine);
            string vCaseType = v_CaseType.ConvertUserVariableToString(engine);
            string vAssignedTo = v_AssignedTo.ConvertUserVariableToString(engine);
            string vDueOn = ((DateTime)v_DueDate.ConvertUserVariableToObject(engine, nameof(v_DueDate), this)).ToString();

            //Trace.WriteLine($"Processing File {fileToProcess}");

            if (!File.Exists(vFileToProcess))
                throw new FileNotFoundException($"ERROR: File not found.{vFileToProcess}");


            DocumentProcessingService dps = CreateAuthenticatedService(engine);

            //// Submits the document to the server for processing
            var docResponse = dps.Submit(vFileToProcess, vTaskQueueName, vName, vDescription, vCaseNumber, vCaseType, vAssignedTo, vDueOn);


            //// Retrieve the ID of the submitted document for further querying and retrieval.
            var humanTaskId = new Guid( docResponse.humanTaskID); // new Guid("00ea6030-40ce-4495-8ef2-418eb0845e62");// 


            string status = dps.GetStatus(humanTaskId);

            humanTaskId.StoreInUserVariable(engine, v_TaskID, nameof(v_TaskID), this);
            status.StoreInUserVariable(engine, v_Status, nameof(v_TaskID), this);
        }
    }
}
