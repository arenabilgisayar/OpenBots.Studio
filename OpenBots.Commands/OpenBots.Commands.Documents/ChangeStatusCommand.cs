using OpenBots.Commands.Documents.Library;
using OpenBots.Commands.Documents.Models;
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
    [Description("Changes the status of the document/task. eg. Change Status to AwaitVerification for Human Review.")]
    public class ChangeStatusCommand : DocumentsBaseCommand //IGetStatusRequest, IGetStatusResult
    {
        [Required]
        [DisplayName("TaskID")]
        [Description("Task Identifier that was provided while submiting the document.")]
        [SampleUsage("1234 || {vTaskId}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_TaskID { get; set; }  //Guid

        [Required]
        [DisplayName("Status")]
        [Description("Status to change to.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Status { get; set; } //TaskStatusTypes

        public ChangeStatusCommand()
        {
            CommandName = "ChangeStatusCommand";
            SelectionName = "Change Status";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;

            v_Status = "Created";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskID = Guid.Parse(v_TaskID.ConvertUserVariableToString(engine));

            DocumentProcessingService ds = CreateAuthenticatedService(engine);

            ds.ChangeStatus(vTaskID, (TaskStatusTypes)Enum.Parse(typeof(TaskStatusTypes), v_Status));
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //DocumentsBaseCommand Inputs
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Username", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TenantId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ApiKey", this, editor));

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TaskID", this, editor));

            var terminalKeyNameLabel = commandControls.CreateDefaultLabelFor("v_Status", this);
            var terminalKeyNameComboBox = commandControls.CreateDropdownFor("v_Status", this);
            terminalKeyNameComboBox.DataSource = Enum.GetValues(typeof(TaskStatusTypes));

            RenderedControls.Add(terminalKeyNameLabel);
            RenderedControls.Add(terminalKeyNameComboBox);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [TaskId '{v_TaskID}' - Status '{v_Status}']";
        }
    }
}
