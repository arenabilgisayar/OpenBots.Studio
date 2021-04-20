﻿using Microsoft.Office.Interop.Outlook;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Loop
{
	[Serializable]
	[Category("Loop Commands")]
	[Description("This command iterates over a collection to let user perform actions on the collection items.")]
	public class LoopCollectionCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input Collection")]
		[Description("Provide a collection variable.")]
		[SampleUsage("{vMyCollection}")]
		[Remarks("If the collection is a DataTable then the output item will be a DataRow and its column value can be accessed using the " +
			"dot operator like {vDataRow.ColumnName}.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DataTable), typeof(List<>), typeof(Dictionary<,>), typeof(string) })]
		public string v_LoopParameter { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Collection Item Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataRow), typeof(IWebElement), typeof(MailItem), typeof(MimeMessage), typeof(KeyValuePair<,>), typeof(string)})]
		public string v_OutputUserVariableName { get; set; }

		public LoopCollectionCommand()
		{
			CommandName = "LoopCollectionCommand";
			SelectionName = "Loop Collection";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			LoopCollectionCommand loopCommand = (LoopCollectionCommand)parentCommand.ScriptCommand;
			var engine = (IAutomationEngineInstance)sender;

			int loopTimes;
			var complexVariable = await v_LoopParameter.EvaluateCode(engine, nameof(v_LoopParameter), this);           

			//if still null then throw exception
			if (complexVariable == null)
			{
				throw new System.Exception("Complex Variable '" + v_LoopParameter + 
					"' not found. Ensure the variable exists before attempting to modify it.");
			}

			dynamic listToLoop;
			if (complexVariable is List<string>)
			{
				listToLoop = (List<string>)complexVariable;
			}
			else if (complexVariable is List<IWebElement>)
			{
				listToLoop = (List<IWebElement>)complexVariable;
			}
			else if (complexVariable is DataTable)
			{
				listToLoop = ((DataTable)complexVariable).Rows;
			}
			else if (complexVariable is List<MailItem>)
			{
				listToLoop = (List<MailItem>)complexVariable;
			}
			else if (complexVariable is List<MimeMessage>)
			{
				listToLoop = (List<MimeMessage>)complexVariable;
			}
			else if (complexVariable is Dictionary<string, string>)
			{
                listToLoop = ((Dictionary<string, string>)complexVariable).ToList();
            }
			else if (complexVariable is Dictionary<string, DataTable>)
			{
				listToLoop = ((Dictionary<string, DataTable>)complexVariable).ToList();
			}
			else if (complexVariable is Dictionary<string, MailItem>)
			{
				listToLoop = ((Dictionary<string, MailItem>)complexVariable).ToList();
			}
			else if (complexVariable is Dictionary<string, MimeMessage>)
			{
				listToLoop = ((Dictionary<string, MimeMessage>)complexVariable).ToList();
			}
			else if (complexVariable is Dictionary<string, IWebElement>)
			{
				listToLoop = ((Dictionary<string, IWebElement>)complexVariable).ToList();
			}
			else if (complexVariable is Dictionary<string, object>)
			{
				listToLoop = ((Dictionary<string, object>)complexVariable).ToList();
			}
			else if ((complexVariable.ToString().StartsWith("[")) && 
				(complexVariable.ToString().EndsWith("]")) && 
				(complexVariable.ToString().Contains(",")))
			{
				//automatically handle if user has given a json array
				JArray jsonArray = JsonConvert.DeserializeObject(complexVariable.ToString()) as JArray;

			   var itemList = new List<string>();
				foreach (var item in jsonArray)
				{
					var value = (JValue)item;
					itemList.Add(value.ToString());
				}

				itemList.SetVariableValue(engine, v_LoopParameter, nameof(v_LoopParameter), this);
				listToLoop = itemList;
			}
			else
				throw new System.Exception("Complex Variable List Type<T> Not Supported");

			loopTimes = listToLoop.Count;

			for (int i = 0; i < loopTimes; i++)
			{
				engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);
				
				((object)listToLoop[i]).SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);

				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopContinuing = false;
						break;
					}
				}

				engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return $"Loop Collection '{v_LoopParameter}' - Store Collection Item in '{v_OutputUserVariableName}'";
		}
	}
}