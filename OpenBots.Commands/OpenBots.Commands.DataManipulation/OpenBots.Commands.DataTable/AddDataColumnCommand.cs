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
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command adds a DataColumn to a DataTable.")]
	public class AddDataColumnCommand : ScriptCommand
	{
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter an existing DataTable to add a DataColumn to.")]
		[SampleUsage("{vDataTable}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("Column Name")]
		[Description("Enter the name of the DataColumn to be added.")]
		[SampleUsage("Column1 || {vColumnName}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ColumnName { get; set; }

		public AddDataColumnCommand()
		{
			CommandName = "AddDataColumnCommand";
			SelectionName = "Add DataColumn";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var dataTableVariable = v_DataTable.ConvertUserVariableToObject(engine, nameof(v_DataTable), this);
			OBDataTable dataTable = (OBDataTable)dataTableVariable;
			var ColumnName = v_ColumnName.ConvertUserVariableToString(engine);

			dataTable.Columns.Add(ColumnName);
			dataTable.StoreInUserVariable(engine, v_DataTable, nameof(v_DataTable), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ColumnName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Add Column {v_ColumnName} to '{v_DataTable}']";
		}
	}
}
