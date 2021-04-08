using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextXtractor.Activities.Model;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Change Status")]
    [Description("Changes the status of the document/task. eg. Change Status to AwaitVerification for Human Review.")]
    public class ChangeStatus : BaseActivity //, IGetStatusRequest, IGetStatusResult
    {
        [Category("Input")]
        [DisplayName("TaskID")]
        [Description("Task Identifier that was provided while submiting the document.")]
        public InArgument<Guid> TaskID { get; set; }


        [Category("Input")]
        [DisplayName("Status")]
        [Description("Status to change to.")]
        public InArgument<TaskStatusTypes> Status { get; set; }

        protected override void Execute(CodeActivityContext context)
        {

            DocumentProcessingService ds = CreateAuthenticatedService(context);

            Guid taskid = TaskID.Get(context);
            TaskStatusTypes newStatus = Status.Get(context);
            ds.ChangeStatus(taskid, newStatus);

        }
    }
}
