﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Switch
{
	[Serializable]
	[Category("Switch Commands")]
	[Description("This command defines a switch/case block which will execute the associated case block if the specified value is a match.")]
	public class BeginSwitchCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Switch")]
		[Description("This value will determine the Case block to execute.")]
		[SampleUsage("{vSwitch}")]
		[Remarks("This value must be a variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_SwitchValue { get; set; }

		public BeginSwitchCommand()
		{
			CommandName = "BeginSwitchCommand";
			SelectionName = "Switch";
			CommandEnabled = true;
			CommandIcon = Resources.command_begin_switch;
			ScopeStartCommand = true;
		}

		public async override void RunCommand(object sender, ScriptAction parentCommand)
		{
			//get engine
			var engine = (IAutomationEngineInstance)sender;

			var vSwitchValue = (string)await v_SwitchValue.EvaluateCode(engine);
			if (vSwitchValue == v_SwitchValue || !v_SwitchValue.StartsWith("{") || !v_SwitchValue.EndsWith("}"))
				throw new Exception("Switch value is not a variable");

			//get indexes of commands
			var caseIndices = FindAllCaseIndices(parentCommand.AdditionalScriptCommands);
			var endSwitchIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is EndSwitchCommand);

			var targetCaseIndex = -1;
			var defaultCaseIndex = -1;
  
			ScriptAction caseCommandItem;
			CaseCommand targetCaseCommand;

			// get index of target case
			foreach (var caseIndex in caseIndices)
			{
				caseCommandItem = parentCommand.AdditionalScriptCommands[caseIndex];
				targetCaseCommand = (CaseCommand)caseCommandItem.ScriptCommand;

				// Save Default Case Index
				if (targetCaseCommand.v_CaseValue == "Default")
				{
					defaultCaseIndex = caseIndex;
				}
				// Save index if the value of any Case matches with the Switch value
				if (vSwitchValue == targetCaseCommand.v_CaseValue)
				{
					targetCaseIndex = caseIndex;
					break;
				}
			}

			// If Target Case Found
			if (targetCaseIndex != -1)
			{
				ExecuteTargetCaseBlock(sender, parentCommand, targetCaseIndex, endSwitchIndex);
			}
			// Else execute Default block
			else if (defaultCaseIndex != -1)
			{
				ExecuteTargetCaseBlock(sender, parentCommand, defaultCaseIndex, endSwitchIndex);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SwitchValue", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $"({v_SwitchValue})";
		}

		private List<int> FindAllCaseIndices(List<ScriptAction> additionalCommands)
		{
			// get the count of all (Enabled) Case Commands
			int totalCaseCommands = additionalCommands.FindAll(
				action => action.ScriptCommand is CaseCommand &&
				action.ScriptCommand.IsCommented == false
				).Count;

			if (totalCaseCommands == 0)
			{
				return null;
			}
			else
			{
				List<int> caseIndices = new List<int>();
				int startIndex = 0;

				// get the indices of all (Enabled) Case Commands
				while (startIndex < additionalCommands.Count && (startIndex = additionalCommands.FindIndex(
					startIndex, (a => a.ScriptCommand is CaseCommand && a.ScriptCommand.IsCommented == false))) != -1)
				{
					caseIndices.Add(startIndex++);
				}

				return caseIndices;
			}
		}

		private int FindNextCaseIndex(List<int> cases, int currCase)
		{
			int nextCase;
			var currentCaseIndex = cases.IndexOf(currCase);

			try
			{
				nextCase = cases[currentCaseIndex + 1];
			}
			catch (Exception)
			{
				nextCase = currCase;
			}

			return nextCase;
		}

		private void ExecuteTargetCaseBlock(object sender, ScriptAction parentCommand, int startCaseIndex, int endCaseIndex)
		{
			//get engine
			var engine = (IAutomationEngineInstance)sender;
			var caseIndices = FindAllCaseIndices(parentCommand.AdditionalScriptCommands);

			// Next Case Index
			var nextCaseIndex = FindNextCaseIndex(caseIndices, startCaseIndex);

			// If Next Case Exist
			if (nextCaseIndex != startCaseIndex)
			{
				// Next Case will be the end of the Target Case
				endCaseIndex = nextCaseIndex;
			}

			// Execute Target Case Block
			for (var caseIndex = startCaseIndex; caseIndex < endCaseIndex; caseIndex++)
			{
				if (engine.IsCancellationPending || engine.CurrentLoopCancelled)
					return;

				engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[caseIndex]);
			}
		}
	}
}