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
    [Category("Documents Commands")]
    [Description("This command changes the status of the document/task. Eg. Change Status to AwaitVerification for Human Review.")]
    public class ChangeStatusCommand : DocumentsBaseCommand //IGetStatusRequest, IGetStatusResult
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
        [DisplayName("Task Status")]
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
            var vTaskID = Guid.Parse(v_TaskId.ConvertUserVariableToString(engine));

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

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TaskId", this, editor));

            var terminalKeyNameLabel = commandControls.CreateDefaultLabelFor("v_Status", this);
            var terminalKeyNameComboBox = commandControls.CreateDropdownFor("v_Status", this);
            terminalKeyNameComboBox.DataSource = Enum.GetValues(typeof(TaskStatusTypes));

            RenderedControls.Add(terminalKeyNameLabel);
            RenderedControls.Add(terminalKeyNameComboBox);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [TaskId '{v_TaskId}' - Task Status '{v_Status}']";
        }
    }
}
