using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Category("OpenBots Documents")]
    [DisplayName("Mark Document As Verified")]
    [Description("Marks a specific document in a bundle as Verfied. Remaining documents typically go for Human Review")]

    public class MarkDocumentAsVerified : DocumentsBaseCommand
    {
        [Category("Input")]
        [DisplayName("TaskID")]
        [Required]
        [Description("Task Identifier that was provided while submiting the document.")]
        public string v_TaskID { get; set; } //Guid


        [Category("Input")]
        [DisplayName("DocumentID")]
        [Required]
        [Description("Document Identifier that was provided while retrieve the processing results.")]
        public string v_DocumentID { get; set; } //Guid

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            DocumentProcessingService service = CreateAuthenticatedService(engine);

            var humanTaskId = Guid.Parse(v_TaskID.ConvertUserVariableToString(engine));
            var docId = Guid.Parse(v_DocumentID.ConvertUserVariableToString(engine));

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

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" []";
        }
    }
}
