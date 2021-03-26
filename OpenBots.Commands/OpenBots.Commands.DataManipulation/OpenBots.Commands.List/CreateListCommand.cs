﻿using Microsoft.Office.Interop.Outlook;
using MimeKit;
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
using Exception = System.Exception;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List
{
	[Serializable]
	[Category("List Commands")]
	[Description("This command creates a new List variable.")]
	public class CreateListCommand : ScriptCommand
	{
		[Required]
		[DisplayName("List Type")]
		[PropertyUISelectionOption("String")]
		[PropertyUISelectionOption("DataTable")]
		[PropertyUISelectionOption("MailItem (Outlook)")]
		[PropertyUISelectionOption("MimeMessage (IMAP/SMTP)")]
		[PropertyUISelectionOption("IWebElement")]
		[Description("Specify the data type of the List to be created.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ListType { get; set; }

		[DisplayName("List Item(s) (Optional)")]
		[Description("Enter the item(s) to write to the List.")]
		[SampleUsage("Hello || {vItem} || Hello,World || {vItem1},{vItem2}")]
		[Remarks("List item can only be a String, DataTable, MailItem or IWebElement.\n" +
				 "Multiple items should be delimited by a comma(,). This input is optional.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(OBDataTable), typeof(MailItem), typeof(MimeMessage), typeof(IWebElement) }, true)]
		public string v_ListItems { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_OutputUserVariableName { get; set; }

		public CreateListCommand()
		{
			CommandName = "CreateListCommand";
			SelectionName = "Create List";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

			v_ListType = "String";
		}

		public override void RunCommand(object sender)
		{
			//get sending instance
			var engine = (IAutomationEngineInstance)sender;
			dynamic vNewList = null;
			string[] splitListItems = null;

			if (!string.IsNullOrEmpty(v_ListItems))
			{
				splitListItems = v_ListItems.Split(',');
			}

			switch (v_ListType)
			{
				case "String":
					vNewList = new List<string>();
					if (splitListItems != null)
					{
						foreach (string item in splitListItems)
							((List<string>)vNewList).Add(item.ConvertUserVariableToString(engine));
					}                   
					break;
				case "DataTable":
					vNewList = new List<OBDataTable>();
					if (splitListItems != null)
					{                       
						foreach (string item in splitListItems)
						{
							OBDataTable dataTable;
							var dataTableVariable = item.ConvertUserVariableToObject(engine, typeof(OBDataTable));
							if (dataTableVariable != null && dataTableVariable is OBDataTable)
								dataTable = (OBDataTable)dataTableVariable;
							else
								throw new Exception("Invalid List Item type, please provide valid List Item type.");
							((List<OBDataTable>)vNewList).Add(dataTable);
						}                           
					}
					break;
				case "MailItem (Outlook)":
					vNewList = new List<MailItem>();
					if (splitListItems != null)
					{
						foreach (string item in splitListItems)
						{
							MailItem mailItem;
							var mailItemVariable = item.ConvertUserVariableToObject(engine, typeof(MailItem));
							if (mailItemVariable != null && mailItemVariable is MailItem)
								mailItem = (MailItem)mailItemVariable;
							else
								throw new Exception("Invalid List Item type, please provide valid List Item type.");
							((List<MailItem>)vNewList).Add(mailItem);
						}
					}
					break;
				case "MimeMessage (IMAP/SMTP)":
					vNewList = new List<MimeMessage>();
					if (splitListItems != null)
					{
						foreach (string item in splitListItems)
						{
							MimeMessage mimeMessage;
							var mimeMessageVariable = item.ConvertUserVariableToObject(engine, typeof(MimeMessage));
							if (mimeMessageVariable != null && mimeMessageVariable is MimeMessage)
								mimeMessage = (MimeMessage)mimeMessageVariable;
							else
								throw new Exception("Invalid List Item type, please provide valid List Item type.");
							((List<MimeMessage>)vNewList).Add(mimeMessage);
						}
					}
					break;
				case "IWebElement":
					vNewList = new List<IWebElement>();
					if (splitListItems != null)
					{
						foreach (string item in splitListItems)
						{
							IWebElement webElement;
							var webElementVariable = item.ConvertUserVariableToObject(engine, typeof(IWebElement));
							if (webElementVariable != null && webElementVariable is IWebElement)
								webElement = (IWebElement)webElementVariable;
							else
								throw new Exception("Invalid List Item type, please provide valid List Item type.");
							((List<IWebElement>)vNewList).Add(webElement);
						}
					}
					break;
			}

			((object)vNewList).StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ListType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItems", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Create New List<{v_ListType}> With Item(s) '{v_ListItems}' - Store List<{v_ListType}> in '{v_OutputUserVariableName}']";
		}
	}
}