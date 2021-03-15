using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Infrastructure;
using OpenBots.NetCore.Core.Properties;
using OpenBots.NetCore.Core.Script;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.NetCore.Commands.Misc
{
    [Serializable]
    [Category("Misc Commands")]
    [Description("This command groups multiple actions together.")]
    public class SequenceCommand : ScriptCommand, ISequenceCommand
    {
        [Browsable(false)]
        public List<ScriptCommand> ScriptActions { get; set; } = new List<ScriptCommand>();

        public SequenceCommand()
        {
            CommandName = "SequenceCommand";
            SelectionName = "Sequence Command";
            CommandEnabled = true;
            CommandIcon = Resources.command_sequence;

        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (IAutomationEngineInstance)sender;

            foreach (var item in ScriptActions)
            {
                //exit if cancellation pending
                if (engine.IsCancellationPending)
                    return;

                //only run if not commented
                if (!item.IsCommented)
                    item.RunCommand(engine);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{ScriptActions.Count()} Embedded Commands]";
        }
    }
}