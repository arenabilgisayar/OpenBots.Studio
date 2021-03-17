﻿using OpenBots.NetCore.Core.Attributes.PropertyAttributes;
using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Infrastructure;
using OpenBots.NetCore.Core.Properties;
using OpenBots.NetCore.Core.User32;
using OpenBots.NetCore.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.NetCore.Commands.Misc
{
    [Serializable]
    [Category("Misc Commands")]
    [Description("This command gets text from the user's clipboard.")]
    public class GetClipboardTextCommand : ScriptCommand
    {

        [Required]
        [Editable(false)]
        [DisplayName("Output Clipboard Text Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName { get; set; }

        public GetClipboardTextCommand()
        {
            CommandName = "GetClipboardTextCommand";
            SelectionName = "Get Clipboard Text";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;

        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            User32Functions.GetClipboardText().StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Clipboard Text in '{v_OutputUserVariableName}']";
        }
    }
}