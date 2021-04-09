using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command saves the processing results in a file system folder.")]
    public class SaveDocumentResultsCommand : DocumentsBaseCommand, ISaveRequest//, ISaveResult
    {
        [Required]
        [DisplayName("TaskId")]
        [Description("Task Identifier that was provided while submiting the document.")]
        [SampleUsage("13db91cf-1f65-4a14-a1cc-bf7aff751b83 || {vTaskId}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_TaskId { get; set; }  //Guid

        [Required]
        [DisplayName("Await Completion")]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. " +
                     "Awaiting queries the service for status every 10 seconds until completed.")]
        [SampleUsage("true || {vAwaitCompletion}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_AwaitCompletion { get; set; } //bool

        [Required]
        [DisplayName("Save Page Images")]
        [Description("Allows the service to download Images of each page.")]
        [SampleUsage("true || {vSavePageImages}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_SavePageImages { get; set; } //bool

        [Required]
        [DisplayName("Save Page Text")]
        [Description("Allows the service to download Text of each page.")]
        [SampleUsage("true || {vSavePageText}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_SavePageText { get; set; } //bool

        [Required]
        [DisplayName("Timeout (Seconds)")]
        [Description("Specify how many seconds to wait before throwing an exception.")]
        [SampleUsage("30 || {vSeconds}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Timeout { get; set; } //int

        [Required]
        [DisplayName("Output Folder")]
        [Description("Folder in which the resulting text and documents are saved.")]
        [SampleUsage(@"C:\temp || {ProjectPath}\temp || {vFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_OutputFolder { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Document Status Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName { get; set; } 
        //"Returns the status of the processing."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsCompleted Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName1 { get; set; }
        //"Returns if the document processing was completed."

        [Required]
        [Editable(false)]
        [DisplayName("Output HasFailed Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName2 { get; set; }
        //"Returns if the document processing has errors or has failed."

        [Required]
        [Editable(false)]
        [DisplayName("Output DocumentInfo JSON Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName3 { get; set; }
        //"Returns the documents extracted as an output for this task as a JSON String"

        [Required]
        [Editable(false)]
        [DisplayName("Output DocumentInfo DataTable Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_OutputUserVariableName4 { get; set; }
        //"Returns the documents extracted as an output for this task as a DataTable. Columns are DocumentNumber " +
        //"(int), Schema (string), PageNumbers (string), Folder (string), DocumentId (string), Confidence (double)"

        [Required]
        [Editable(false)]
        [DisplayName("Output Data DataTable Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_OutputUserVariableName5 { get; set; }
        //"Appends the data extracted as an output for this task in a DataTable. Columns are TaskId (string), " +
        //"DocumentId (string), DocumentNumber (int), Schema (string), PageNumbers (string), <fields of from all " +
        //"schemas found>. Simply use 'Write CSV' to save these results."

        public SaveDocumentResultsCommand()
        {
            CommandName = "SaveDocumentResultsCommand";
            SelectionName = "Save Document Results";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;

            v_AwaitCompletion = "false";
            v_SavePageImages = "false";
            v_SavePageText = "false";
            v_Timeout = "120";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            Guid vHumanTaskId = Guid.Parse(v_TaskId.ConvertUserVariableToString(engine));
            bool vAwaitCompletion = bool.Parse(v_AwaitCompletion.ConvertUserVariableToString(engine));
            int vTimeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
            bool vSavePageText = bool.Parse(v_SavePageText.ConvertUserVariableToString(engine));
            bool vSavePageImages = bool.Parse(v_SavePageImages.ConvertUserVariableToString(engine));
            string vOutputFolder = v_OutputFolder.ConvertUserVariableToString(engine);

            DocumentProcessingService service = CreateAuthenticatedService(engine);

            var vData = v_OutputUserVariableName5.ConvertUserVariableToObject(engine, nameof(v_OutputUserVariableName5), this);
            DataTable dataDt = vData == null ? null : (DataTable)vData;
   
            DocumentInfo docInfo =  service.SaveDocumentLocally(vHumanTaskId, vAwaitCompletion, vTimeout, vSavePageText, vSavePageImages, vOutputFolder, 
                                                                ref dataDt, out string status, out bool hasFailed, out bool isCompleted);

            var docInfoAsJSON = docInfo.SerializeJSON();
            var docInfoAsDataTable = docInfo.CreateDataTable();
           
            status.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
            isCompleted.StoreInUserVariable(engine, v_OutputUserVariableName1, nameof(v_OutputUserVariableName1), this);
            hasFailed.StoreInUserVariable(engine, v_OutputUserVariableName2, nameof(v_OutputUserVariableName2), this);
            docInfoAsJSON.StoreInUserVariable(engine, v_OutputUserVariableName3, nameof(v_OutputUserVariableName3), this);
            docInfoAsDataTable.StoreInUserVariable(engine, v_OutputUserVariableName4, nameof(v_OutputUserVariableName4), this);
            dataDt.StoreInUserVariable(engine, v_OutputUserVariableName5, nameof(v_OutputUserVariableName5), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //DocumentsBaseCommand Inputs
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Username", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TenantId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ApiKey", this, editor));
            
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TaskId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AwaitCompletion", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SavePageImages", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SavePageText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_OutputFolder", this, editor));
            
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName1", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName3", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName4", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName5", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Document Status in '{v_OutputUserVariableName}']";
        }
    }
}
