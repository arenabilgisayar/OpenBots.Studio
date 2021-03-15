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
    [Description("This command rethrows an exception caught in a catch block.")]
    public class RethrowCommand : ScriptCommand
    {
        public RethrowCommand()
        {
            CommandName = "RethrowCommand";
            SelectionName = "Rethrow";
            CommandEnabled = true;
            CommandIcon = Resources.command_exception;

        }

        public override void RunCommand(object sender)
        {
            throw new Exception("Rethrowing Original Exception");
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