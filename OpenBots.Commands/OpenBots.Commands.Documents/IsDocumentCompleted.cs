using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System.ComponentModel;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Is Document Completed")]
    [Description("Evaluates Status. Determines if processing is completed based on Status message. Returns Boolean, can use in RetryScope ")]
    public class IsDocumentCompleted : ScriptCommand//CodeActivity<bool>
    {
        [Category("Input")]
        [DisplayName("Document Status")]
        [Description("Status of the task/document submitted for processing. Expect 'Created' or 'InProgress'")]
        public string v_Status { get; set; }

        [Category("Output")]
        [DisplayName("Is Document Completed")]
        [Description("Status of the task/document submitted for processing. Expect 'Created' or 'InProgress'")] //Needs to be edited
        public string v_IsDocumentCompleted { get; set; } //bool

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            
            string vStatus =  v_Status.ConvertUserVariableToString(engine);
            bool isDocumentCompleted;

            if (vStatus == "InProgress" || vStatus == "Created")
                isDocumentCompleted =  false;
            else
                isDocumentCompleted = true;

            isDocumentCompleted.StoreInUserVariable(engine, v_IsDocumentCompleted, nameof(v_IsDocumentCompleted), this);
        }
    }
}
