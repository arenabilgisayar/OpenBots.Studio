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
    [DisplayName("Is Document Completed")]
    [Description("Evaluates Status. Determines if processing is completed based on Status message. Returns Boolean, can use in RetryScope ")]
    public class IsDocumentCompleted : CodeActivity<bool>
    {
        [Category("Input")]
        [DisplayName("Document Status")]
        [Description("Status of the task/document submitted for processing. Expect 'Created' or 'InProgress'")]
        public OutArgument<string> Status { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            string status =  Status.Get(context);

            if (status == "InProgress" || status == "Created")
            {
                return false;
            }
            return true;
        }
    }
}
