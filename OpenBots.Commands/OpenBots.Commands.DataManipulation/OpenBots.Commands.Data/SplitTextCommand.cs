﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command splits a string by a delimiter and saves the result in a list.")]
	public class SplitTextCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text Data")]
		[Description("Provide a variable or text value.")]
		[SampleUsage("Sample text, to be splitted by comma delimiter || {vTextData}")]
		[Remarks("Providing data of a type other than a 'String' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputText { get; set; }

		[Required]
		[DisplayName("Text Delimiter")]
		[Description("Specify the character that will be used to split the text.")]
		[SampleUsage("Environment.Newline || , || vDelimiter || new List<string>(){\";\", \".\"}|| vDelimeterList")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(List<string>) })]
		public string v_SplitCharacter { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_OutputUserVariableName { get; set; }

		public SplitTextCommand()
		{
			CommandName = "SplitTextCommand";
			SelectionName = "Split Text";
			CommandEnabled = true;
			CommandIcon = Resources.command_string;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var stringVariable = (string)await v_InputText.EvaluateCode(engine, nameof(v_InputText), this);
			dynamic input = await v_SplitCharacter.EvaluateCode(engine, nameof(v_SplitCharacter), this);

			List<string> splitString;

			if (input is List<string>)
			{
				List<string> splitCharacterList = (List<string>)input;
				splitString = stringVariable.Split(splitCharacterList.ToArray(), StringSplitOptions.None).ToList();
			}				
			else if (input is string)
            {
				string splitCharacter = (string)input;
				splitString = stringVariable.Split(new string[] { splitCharacter }, StringSplitOptions.None).ToList();
			}
			else
				throw new InvalidDataException($"{v_SplitCharacter} is not a valid delimeter");

			splitString.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);           
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SplitCharacter", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Split '{v_InputText}' by '{v_SplitCharacter}' - Store List in '{v_OutputUserVariableName}']";
		}
	}
}