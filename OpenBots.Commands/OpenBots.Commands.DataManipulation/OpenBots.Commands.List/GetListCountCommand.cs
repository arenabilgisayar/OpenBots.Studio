﻿using Microsoft.Office.Interop.Outlook;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List
{
	[Serializable]
	[Category("List Commands")]
	[Description("This command returns the count of items contained in a List.")]
	public class GetListCountCommand : ScriptCommand
	{
		[Required]
		[DisplayName("List")]
		[Description("Provide a List variable.")]
		[SampleUsage("{vList}")]
		[Remarks("Providing any type of variable other than a List will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_ListName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Count Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		public GetListCountCommand()
		{
			CommandName = "GetListCountCommand";
			SelectionName = "Get List Count";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//get variable by regular name
			var listVariable = v_ListName.ConvertUserVariableToObject(engine, nameof(v_ListName), this);

			//if still null then throw exception
			if (listVariable == null)
			{
				throw new System.Exception("Complex Variable '" + v_ListName +
					"' not found. Ensure the variable exists before attempting to modify it.");
			}

			dynamic listToCount; 
			if (listVariable is List<string>)
				listToCount = (List<string>)listVariable;
			else if (listVariable is List<OBDataTable>)
				listToCount = (List<OBDataTable>)listVariable;
			else if (listVariable is List<MailItem>)
				listToCount = (List<MailItem>)listVariable;
			else if (listVariable is List<MimeMessage>)
				listToCount = (List<MimeMessage>)listVariable;
			else if (listVariable is List<IWebElement>)
				listToCount = (List<IWebElement>)listVariable;
			else if (listVariable.ToString().StartsWith("[") && listVariable.ToString().EndsWith("]") && 
					 listVariable.ToString().Contains(","))
			{
				//automatically handle if user has given a json array
				JArray jsonArray = JsonConvert.DeserializeObject(listVariable.ToString()) as JArray;

				var itemList = new List<string>();
				foreach (var item in jsonArray)
				{
					var value = (JValue)item;
					itemList.Add(value.ToString());
				}

				itemList.StoreInUserVariable(engine, v_ListName, nameof(v_ListName), this);
				listToCount = itemList;
			}
			else
				throw new System.Exception("Complex Variable List Type<T> Not Supported");

			int count = listToCount.Count;
			count.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_ListName}' - Store Count in '{v_OutputUserVariableName}']";
		}       
	}
}