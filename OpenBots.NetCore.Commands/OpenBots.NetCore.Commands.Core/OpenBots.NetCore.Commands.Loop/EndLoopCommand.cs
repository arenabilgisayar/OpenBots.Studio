using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Infrastructure;
using OpenBots.NetCore.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.NetCore.Commands.Loop
{

    [Serializable]
    [Category("Loop Commands")]
    [Description("This command signifies the exit point of Loop command(s) and is required for all the Loop commands.")]
    public class EndLoopCommand : ScriptCommand
    {
        public EndLoopCommand()
        {
            CommandName = "EndLoopCommand";
            SelectionName = "End Loop";
            CommandEnabled = true;
            CommandIcon = Resources.command_endloop;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "End Loop";
        }
    }
}