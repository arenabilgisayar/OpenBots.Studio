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
    [Category("OpenBots Documents")]
    [Description("Marks a specific document in a bundle as Verfied. Remaining documents typically go for Human Review.")]
    public class MarkDocumentAsVerifiedCommand : DocumentsBaseCommand
    {
        [Required]
        [DisplayName("TaskId")]
        [Description("Task Identifier that was provided while submiting the document.")]
        [SampleUsage("13db91cf-1f65-4a14-a1cc-bf7aff751b83 || {vTaskID}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_TaskId { get; set; } //Guid

        [Required]
        [DisplayName("DocumentId")]
        [Description("Document Identifier that was provided while retrieve the processing results.")]
        [SampleUsage("13db91cf-1f65-4a14-a1cc-bf7aff751b83 || {vDocumentId}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_DocumentId { get; set; } //Guid

        public MarkDocumentAsVerifiedCommand()
        {
            CommandName = "MarkDocumentAsVerifiedCommand";
            SelectionName = "Mark Document As Verified";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            DocumentProcessingService service = CreateAuthenticatedService(engine);

            var humanTaskId = Guid.Parse(v_TaskId.ConvertUserVariableToString(engine));
            var docId = Guid.Parse(v_DocumentId.ConvertUserVariableToString(engine));

            service.MarkDocumentAsVerified(humanTaskId, docId);
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DocumentId", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [TaskId '{v_TaskId}' - DocumentId '{v_DocumentId}']";
        }
    }
}
