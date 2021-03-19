﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Switch
{
	[Serializable]
	[Category("Switch Commands")]
	[Description("This command defines a case block whose commands will execute if the value specified in the "+
				 "case is equal to that of the preceding Switch Command.")]
	public class CaseCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Case")]
		[Description("This block will be executed if the specified case value matches the value in the Switch Command.")]
		[SampleUsage("1 || hello")]
		[Remarks("")]
		[CompatibleTypes(null, true)]
		public string v_CaseValue { get; set; }

		public CaseCommand()
		{
			CommandName = "CaseCommand";
			SelectionName = "Case";
			CommandEnabled = true;
			CommandIcon = Resources.command_case;
			ScopeStartCommand = true;
		}

		public override void RunCommand(object sender)
		{
			//no execution required, used as a marker by the Automation Engine
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CaseValue", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_CaseValue == "Default")
				return "Default:";
			else
				return base.GetDisplayValue() + $" {v_CaseValue}:";
		}
	}
}
