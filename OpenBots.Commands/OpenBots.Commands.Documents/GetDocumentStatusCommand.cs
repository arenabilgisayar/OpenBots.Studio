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
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command retrieves the current status of the document being processed. It can also wait for the document's completion.")]
    public class GetDocumentStatusCommand : DocumentsBaseCommand, IGetStatusRequest//, IGetStatusResult
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
        [DisplayName("Timeout (Seconds)")]
        [Description("Specify how many seconds to wait before throwing an exception.")]
        [SampleUsage("30 || {vSeconds}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_Timeout { get; set; } //int

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
        [DisplayName("Output HasError Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName2 { get; set; }
        //"Document Processing has errors and couldnt complete."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsCurrentlyProcessing Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName3 { get; set; }
        //"Document is currently being processed."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsSuccessful Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName4 { get; set; }
        //"Is Document Processing Completed Successfully and read for results data to be read."

        public GetDocumentStatusCommand()
        {
            CommandName = "GetDocumentStatusCommand";
            SelectionName = "Get Document Status";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;

            v_AwaitCompletion = "false";
            v_Timeout = "120";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskId = Guid.Parse(v_TaskId.ConvertUserVariableToString(engine));
            var vAwaitCompletion = bool.Parse(v_AwaitCompletion.ConvertUserVariableToString(engine));

            DocumentProcessingService ds = CreateAuthenticatedService(engine);

            string status = ds.GetStatus(vTaskId);

            if(vAwaitCompletion)
            {
                int vTimeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));
                status = ds.AwaitProcessing(vTaskId, vTimeout);
            }

            bool isCompleted;
            bool hasError = false;
            bool isCurrentlyProcessing = true;
            bool isSuccessful;

            if (status == "Processed")
            {
                isCompleted = true;
                hasError = false;
                isCurrentlyProcessing = false;
                isSuccessful = true;
            }
            else
            {
                isCompleted = false;
                isSuccessful = false;
            }

            if (status == "InProgress")
            {
                hasError = false;
                isCurrentlyProcessing = true;
                isSuccessful = false;
            }

            if (status == "CompletedWithError")
            {
                isCompleted = true;
                hasError = true;
                isCurrentlyProcessing = false;
            }

            status.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
            isCompleted.StoreInUserVariable(engine, v_OutputUserVariableName1, nameof(v_OutputUserVariableName1), this);
            hasError.StoreInUserVariable(engine, v_OutputUserVariableName2, nameof(v_OutputUserVariableName2), this);
            isCurrentlyProcessing.StoreInUserVariable(engine, v_OutputUserVariableName3, nameof(v_OutputUserVariableName3), this);
            isSuccessful.StoreInUserVariable(engine, v_OutputUserVariableName4, nameof(v_OutputUserVariableName4), this);
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
            
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName1", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName3", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName4", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Document Status in '{v_OutputUserVariableName}']";
        }
    }
}
