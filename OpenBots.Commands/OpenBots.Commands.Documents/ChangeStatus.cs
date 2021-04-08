using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Change Status")]
    [Description("Changes the status of the document/task. eg. Change Status to AwaitVerification for Human Review.")]
    public class ChangeStatus : BaseActivity //IGetStatusRequest, IGetStatusResult
    {
        [Category("Input")]
        [DisplayName("TaskID")]
        [Description("Task Identifier that was provided while submiting the document.")]
        public string v_TaskID { get; set; }  //Guid


        [Category("Input")]
        [DisplayName("Status")]
        [Description("Status to change to.")]
        public string v_Status { get; set; } //TaskStatusTypes
        //make dropdown

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskID = Guid.Parse(v_TaskID.ConvertUserVariableToString(engine));

            //set dropdown options as enum


            DocumentProcessingService ds = CreateAuthenticatedService(engine);

            ds.ChangeStatus(vTaskID, (TaskStatusTypes)Enum.Parse(typeof(TaskStatusTypes), v_Status));
        }
    }
}
