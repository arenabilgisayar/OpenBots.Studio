﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Excel
{
	[Serializable]
	[Category("Excel Commands")]
	[Description("This command closes an open Excel Workbook and Instance.")]
	public class ExcelCloseApplicationCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Excel Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Application** command.")]
		[SampleUsage("MyExcelInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Application) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Save Workbook")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Indicate whether the Workbook should be saved before closing.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ExcelSaveOnExit { get; set; }

		public ExcelCloseApplicationCommand()
		{
			CommandName = "ExcelCloseApplicationCommand";
			SelectionName = "Close Excel Application";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_InstanceName = "DefaultExcel";
			v_ExcelSaveOnExit = "Yes";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var excelObject = v_InstanceName.GetAppInstance(engine);
			var excelInstance = (Application)excelObject;
			bool saveOnExit;
			if (v_ExcelSaveOnExit == "Yes")
				saveOnExit = true;
			else
				saveOnExit = false;

			//check if workbook exists and save
			if (excelInstance.ActiveWorkbook != null)
			{
				excelInstance.ActiveWorkbook.Close(saveOnExit);
			}

			//close excel
			excelInstance.Quit();
			//remove instance
			v_InstanceName.RemoveAppInstance(engine);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ExcelSaveOnExit", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Save on Close '{v_ExcelSaveOnExit}' - Instance Name '{v_InstanceName}']";
		}
	}
}