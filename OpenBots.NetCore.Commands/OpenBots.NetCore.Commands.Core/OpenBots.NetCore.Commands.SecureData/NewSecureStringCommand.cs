﻿using OpenBots.NetCore.Core.Attributes.PropertyAttributes;
using OpenBots.NetCore.Core.Command;
using OpenBots.NetCore.Core.Enums;
using OpenBots.NetCore.Core.Infrastructure;
using OpenBots.NetCore.Core.Properties;
using OpenBots.NetCore.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Forms;

namespace OpenBots.NetCore.Commands.SecureData
{
	[Serializable]
	[Category("Secure Data Commands")]
	[Description("This command adds text as a SecureString into a variable.")]
	public class NewSecureStringCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input Text")]
		[Description("Enter the text for the variable.")]
		[SampleUsage("Some Text || {vText}")]
		[Remarks("You can use variables in input if you encase them within braces {vText}. You can also perform basic math operations.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Input { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output SecureString Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_OutputUserVariableName { get; set; }

		public NewSecureStringCommand()
		{
			CommandName = "NewSecureStringCommand";
			SelectionName = "New SecureString";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;

		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			SecureString secureStringValue = v_Input.ConvertUserVariableToString(engine).ConvertStringToSecureString();

			secureStringValue.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_Input", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Store SecureString in '{v_OutputUserVariableName}']";
		}
	}
}