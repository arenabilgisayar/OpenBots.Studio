using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
using OpenBots.Core.Infrastructure;
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
    [Category("OpenBots Documents")]
    [Description("Saves the processing results in a file system folder.")]
    public class SaveDocumentResults : DocumentsBaseCommand, ISaveRequest, ISaveResult
    {

        [Category("Input")]
        [DisplayName("TaskID")]
        [Required]
        [Description("Task Identifier that was provided while submiting the document.")]
        public string v_TaskID { get; set; }  //Guid

        [Category("Input")]
        [DisplayName("Await Completion")]
        [DefaultValue(false)]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. Awaiting queries the service for status every 10 seconds until completed.")]
        public string v_AwaitCompletion { get; set; } //bool

        [Category("Input")]
        [DisplayName("Save Page Images")]
        [DefaultValue(false)]
        [Description("Allows the service to download Images of each page.")]
        public string v_SavePageImages { get; set; } //bool

        [Category("Input")]
        [DisplayName("Save Page Text")]
        [DefaultValue(false)]
        [Description("Allows the service to download Text of each page.")]
        public string v_SavePageText { get; set; } //bool

        [Category("Input")]
        [DisplayName("Timeout (in seconds)")]
        [DefaultValue(120)]
        [Description("Timeout if awaiting for document processing to be completed.")]
        public string v_Timeout { get; set; } //int

        [Category("Input")]
        [DisplayName("Output Folder")]
        [Description("Folder in which the resulting text and documents are saved.")]
        [Required]
        public string v_OutputFolder { get; set; }

        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Returns the status of the processing.")]
        public string v_Status { get; set; }

        [Category("Output")]
        [DisplayName("Is Document Process Completed")]
        [Description("Returns if the document processing was completed.")]
        public string v_IsCompleted { get; set; } //bool

        [Category("Output")]
        [DisplayName("Has Failed or Has Errors")]
        [Description("Returns if the document processing has errors or has failed.")]
        public string v_HasFailedOrError { get; set; } //bool

        [Category("Output")]
        [DisplayName("OutputAsJSON")]
        [Description("Returns the documents extracted as an output for this task as a JSON String")]
        public string v_OutputAsJSON { get; set; }

        [Category("Output")]
        [DisplayName("OutputAsTable")]
        [Description("Returns the documents extracted as an output for this task as a DataTable. Columns are DocumentNumber (int), Schema (string), PageNumbers (string), Folder (string), DocumentId (string), Confidence (double)")]
        public string v_OutputAsTable { get; set; } //DataTable


        [Category("Output")]
        [DisplayName("DataAsTable")]
        [Description("Appends the data extracted as an output for this task in a DataTable. Columns are TaskId (string), DocumentId (string), DocumentNumber (int), Schema (string), PageNumbers (string), <fields of from all schemas found>. Simply use 'Write CSV' to save these results.")]
        public string v_DataAsTable { get; set; } //DataTable

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            DocumentProcessingService service = CreateAuthenticatedService(engine);

            Guid vHumanTaskId = Guid.Parse(v_TaskID.ConvertUserVariableToString(engine));
            bool vAwaitCompletion = bool.Parse(v_AwaitCompletion.ConvertUserVariableToString(engine));
            int vTimeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
            bool vSavePageText = bool.Parse(v_SavePageText.ConvertUserVariableToString(engine));
            bool vSavePageImages = bool.Parse(v_SavePageImages.ConvertUserVariableToString(engine));
            string vOutputFolder = v_OutputFolder.ConvertUserVariableToString(engine);
            DataTable vDataTable = (DataTable)v_DataAsTable.ConvertUserVariableToObject(engine, nameof(v_DataAsTable), this);

            
            DocumentInfo docInfo =  service.SaveDocumentLocally(vHumanTaskId, vAwaitCompletion, vTimeout, vSavePageText, vSavePageImages, vOutputFolder, 
                                                                ref vDataTable, out string status, out bool hasFailed, out bool isCompleted);

            var docInfoAsJSON = docInfo.SerializeJSON();
            var docInfoAsDataTable = docInfo.CreateDataTable();

            docInfoAsJSON.StoreInUserVariable(engine, v_OutputAsJSON, nameof(v_OutputAsJSON), this);
            docInfoAsDataTable.StoreInUserVariable(engine, v_OutputAsTable, nameof(v_OutputAsTable), this);
            vDataTable.StoreInUserVariable(engine, v_DataAsTable, nameof(v_DataAsTable), this);
            status.StoreInUserVariable(engine, v_Status, nameof(v_Status), this);
            isCompleted.StoreInUserVariable(engine, v_IsCompleted, nameof(v_IsCompleted), this);
            hasFailed.StoreInUserVariable(engine, v_HasFailedOrError, nameof(v_HasFailedOrError), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //DocumentsBaseCommand Inputs
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Username", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TenantId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ApiKey", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" []";
        }
    }
}
