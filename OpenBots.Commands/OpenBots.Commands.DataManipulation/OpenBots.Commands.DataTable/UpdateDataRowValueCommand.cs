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
using System.Data;
using System.Windows.Forms;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command updates a Value in a DataRow at a specified column name/index.")]

	public class UpdateDataRowValueCommand : ScriptCommand
	{

		[Required]
		[DisplayName("DataRow")]
		[Description("Enter an existing DataRow to add values to.")]
		[SampleUsage("{vDataRow}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DataRow) })]
		public string v_DataRow { get; set; }

		[Required]
		[DisplayName("Search Option")]
		[PropertyUISelectionOption("Column Name")]
		[PropertyUISelectionOption("Column Index")]
		[Description("Select whether the DataRow value should be found by column index or column name.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Search Value")]
		[Description("Enter a valid DataRow index or column name.")]
		[SampleUsage("0 || vIndex || \"Column1\" || vColumnName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_DataValueIndex { get; set; }

		[Required]
		[DisplayName("Cell Value")]
		[Description("Enter the value to write to the DataRow cell.")]
		[SampleUsage("value || {vValue}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_DataRowValue { get; set; }

		public UpdateDataRowValueCommand()
		{
			CommandName = "UpdateDataRowValueCommand";
			SelectionName = "Update DataRow Value";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_Option = "Column Index";
		}

		public async override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var dataRowValue = (string)await v_DataRowValue.EvaluateCode(engine);

			var dataRowVariable = await v_DataRow.EvaluateCode(engine, nameof(v_DataRow), this);
			DataRow dataRow = (DataRow)dataRowVariable;

			dynamic valueIndex = await v_DataValueIndex.EvaluateCode(engine);

			if (v_Option == "Column Index")
			{
				int index = int.Parse(valueIndex);
				dataRow[index] = dataRowValue;
			}
			else if (v_Option == "Column Name")
			{
				string index = valueIndex;
				dataRow.SetField(index, dataRowValue);
			}
			dataRow.SetVariableValue(engine, v_DataRow, typeof(DataRow));
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRow", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRowValue", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Write '{v_DataRowValue}' to Column '{v_DataValueIndex}' in '{v_DataRow}']";
		}       
	}
}