using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command submits a file for processing by creating a new Task.")]
    public class SubmitDocumentCommand : DocumentsBaseCommand, ISubmitFileRequest//, ISubmitFileResult
    {
        [Required]
        [DisplayName("File Path")]
        [Description("Path of the file to be submitted.")]
        [SampleUsage(@"C:\temp\myfile.pdf || {ProjectPath}\myfile.pdf || {vTextFilePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_FilePath { get; set; }

        [DisplayName("Task Queue Name (Optional)")]
        [Description("Name of the Queue that this task would be created in. If unspecified, it will be defauted to the 'Common' queue.")]
        [SampleUsage("0 || {vIndex}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_QueueName { get; set; }

        [Required]
        [DisplayName("Task Name/Title")]
        [Description("Name or Title of the task to be created.")]
        [SampleUsage("My OB Document || {vTaskName}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Name { get; set; }

        [DisplayName("Description (Optional)")]
        [Description("Description of the task for Reviewers or other downstream processing.")]
        [SampleUsage("Hello World || {vDescription}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Description { get; set; }

        [DisplayName("Case Number (Optional)")]
        [Description("A case number for reference.")]
        [SampleUsage("123 || {vCaseNumber}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_CaseNumber { get; set; }

        [DisplayName("Case Type (Optional)")]
        [Description("A case type for reference.")]
        [SampleUsage("Test || {vCaseType}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_CaseType { get; set; }

        [DisplayName("Assign To User (Optional)")]
        [Description("Email address of the user to assign the task.")]
        [SampleUsage("user@openbots.ai || {vUser}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_AssignedTo { get; set; }

        [DisplayName("Task Due Date (Optional)")]
        [Description("Due Date for the Task.")]
        [SampleUsage("1/1/2000 || {vDate} || {DateTime.Now}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(DateTime), typeof(string) })]
        public string v_DueDate { get; set; } //DateTime

        [Required]
        [Editable(false)]
        [DisplayName("Output TaskId Guid Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(Guid) })]
        public string v_OutputUserVariableName { get; set; } //"A identification number for the task created that can be used for subsequent calls / queries."

        [Required]
        [Editable(false)]
        [DisplayName("Output Document Status Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName2 { get; set; } //"Status of the task just after submitting the document. Expect 'Created' or 'InProgress'"

        public SubmitDocumentCommand()
        {
            CommandName = "SubmitDocumentCommand";
            SelectionName = "Submit Document";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;
        }

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

            dynamic inputDate = v_DueDate.ConvertUserVariableToString(engine);
            if (inputDate == v_DueDate && inputDate.StartsWith("{") && inputDate.EndsWith("}"))
                inputDate = v_DueDate.ConvertUserVariableToObject(engine, nameof(v_DueDate), this);

            string vDueOnStr;

            if (inputDate is DateTime)
                vDueOnStr = ((DateTime)inputDate).ToString();
            else if (inputDate is string)
                vDueOnStr = DateTime.Parse((string)inputDate).ToString();
            else
                throw new InvalidDataException($"{v_DueDate} is not a valid DateTime");

            //Trace.WriteLine($"Processing File {fileToProcess}");

            if (!File.Exists(vFileToProcess))
                throw new FileNotFoundException($"ERROR: File not found. {vFileToProcess}");

            DocumentProcessingService dps = CreateAuthenticatedService(engine);

            //// Submits the document to the server for processing
            var docResponse = dps.Submit(vFileToProcess, vTaskQueueName, vName, vDescription, vCaseNumber, vCaseType, vAssignedTo, vDueOnStr);

            //// Retrieve the ID of the submitted document for further querying and retrieval.
            var humanTaskId = Guid.Parse(docResponse.humanTaskID); // new Guid("00ea6030-40ce-4495-8ef2-418eb0845e62");// 

            string status = dps.GetStatus(humanTaskId);

            humanTaskId.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
            status.StoreInUserVariable(engine, v_OutputUserVariableName2, nameof(v_OutputUserVariableName2), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //DocumentsBaseCommand Inputs
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Username", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TenantId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ApiKey", this, editor));

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Name", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Description", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CaseNumber", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CaseType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AssignedTo", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DueDate", this, editor));

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store TaskId in '{v_OutputUserVariableName}' - Store Document Status in '{v_OutputUserVariableName2}']";
        }
    }
}
