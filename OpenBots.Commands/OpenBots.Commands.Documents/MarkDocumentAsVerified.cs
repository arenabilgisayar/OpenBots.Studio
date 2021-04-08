using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Mark Document As Verified")]
    [Description("Marks a specific document in a bundle as Verfied. Remaining documents typically go for Human Review")]

    public class MarkDocumentAsVerified : BaseActivity
    {
        [Category("Input")]
        [DisplayName("TaskID")]
        [RequiredArgument]
        [Description("Task Identifier that was provided while submiting the document.")]
        public InArgument<Guid> TaskID { get; set; }


        [Category("Input")]
        [DisplayName("DocumentID")]
        [RequiredArgument]
        [Description("Document Identifier that was provided while retrieve the processing results.")]
        public InArgument<Guid> DocumentID { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            DocumentProcessingService service = CreateAuthenticatedService(context);

            var humanTaskId = TaskID.Get(context);
            var docId = DocumentID.Get(context);

            service.MarkDocumentAsVerified(humanTaskId, docId);
        }
    }
}
