using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Mark Document As Verified")]
    [Description("Marks a specific document in a bundle as Verfied. Remaining documents typically go for Human Review")]

    public class MarkDocumentAsVerified : BaseActivity
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
    }
}
