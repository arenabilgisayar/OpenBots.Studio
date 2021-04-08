using OpenBots.Commands.Documents.Interfaces;
using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("OpenBots Documents")]
    [Description("This command retrieves the current status of the document being processed. It can also wait for the document's completion.")]
    public class GetDocumentStatus : DocumentsBaseCommand, IGetStatusRequest, IGetStatusResult
    {

        [Category("Input")]
        [DisplayName("TaskID")]
        [Description("Task Identifier that was provided while submiting the document.")]
        public string v_TaskID { get; set; }  //Guid

        [Category("Input")]
        [DisplayName("Await Completion")]
        [DefaultValue(false)]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. " +
                     "Awaiting queries the service for status every 10 seconds until completed.")]
        public string v_AwaitCompletion { get; set; } //bool

        [Category("Input")]
        [DisplayName("Timeout (in seconds)")]
        [DefaultValue(120)]
        [Description("Timeout if awaiting for document processing to be completed.")]
        public string v_Timeout { get; set; } //int Timeout in seconds


        [Category("Output")]
        [DisplayName("Document Status")]
        [Description("Status of the task/document submitted for processing. Expect 'Created' or 'InProgress'")]
        public string v_Status { get; set; }

        [Category("Output")]
        [DisplayName("Is Document Completed")]
        [Description("True if the document has finished Processing")]
        public string v_IsDocumentCompleted { get; set; } //bool

        [Category("Output")]
        [DisplayName("Has Error")]
        [Description("Document Processing has errors and couldnt complete.")]
        public string v_HasError { get; set; } //bool

        [Category("Output")]
        [DisplayName("Is Currently Processing")]
        [Description("Document is currently being processed")]
        public string v_IsCurrentlyProcessing { get; set; } //bool

        [Category("Output")]
        [DisplayName("Is Successful")]
        [Description("Is Document Processing Completed Successfully and read for results data to be read")]
        public string v_IsSuccessful { get; set; } //bool


        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskID = Guid.Parse(v_TaskID.ConvertUserVariableToString(engine));
            var vAwaitCompletion = bool.Parse(v_AwaitCompletion.ConvertUserVariableToString(engine));

            DocumentProcessingService ds = CreateAuthenticatedService(engine);

            string status = ds.GetStatus(vTaskID);

            if(vAwaitCompletion)
            {
                int vTimeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
                status = ds.AwaitProcessing(vTaskID, vTimeout);
            }

            status.StoreInUserVariable(engine, v_Status, nameof(v_Status), this);

            if (status == "Processed")
            {
                true.StoreInUserVariable(engine, v_IsDocumentCompleted, nameof(v_IsDocumentCompleted), this);
                false.StoreInUserVariable(engine, v_HasError, nameof(v_HasError), this);
                false.StoreInUserVariable(engine, v_IsCurrentlyProcessing, nameof(v_IsCurrentlyProcessing), this);
                true.StoreInUserVariable(engine, v_IsSuccessful, nameof(v_IsSuccessful), this);
            }
            else
            {
                false.StoreInUserVariable(engine, v_IsSuccessful, nameof(v_IsSuccessful), this);
                false.StoreInUserVariable(engine, v_IsDocumentCompleted, nameof(v_IsDocumentCompleted), this);
            }

            if (status == "InProgress")
            {
                false.StoreInUserVariable(engine, v_HasError, nameof(v_HasError), this);
                true.StoreInUserVariable(engine, v_IsCurrentlyProcessing, nameof(v_IsCurrentlyProcessing), this);
                false.StoreInUserVariable(engine, v_IsSuccessful, nameof(v_IsSuccessful), this);
            }

            if (status == "CompletedWithError")
            {
                true.StoreInUserVariable(engine, v_IsDocumentCompleted, nameof(v_IsDocumentCompleted), this);
                true.StoreInUserVariable(engine, v_HasError, nameof(v_HasError), this);
                false.StoreInUserVariable(engine, v_IsCurrentlyProcessing, nameof(v_IsCurrentlyProcessing), this);
            }
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
