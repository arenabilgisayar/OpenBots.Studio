using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Infrastructure;
using OpenBots.NetCore.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.NetCore.Commands.ErrorHandling
{
    [Serializable]
    [Category("Error Handling Commands")]
    [Description("This command specifies the end of a retry block.")]
    public class EndRetryCommand : ScriptCommand
    {
        public EndRetryCommand()
        {
            CommandName = "EndRetryCommand";
            SelectionName = "End Retry";
            CommandIcon = Resources.command_end_try;

            CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}
