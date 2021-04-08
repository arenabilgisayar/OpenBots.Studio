using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
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

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("OpenBots Documents")]
    [Description("This command compares two tables cell by cell and reports any mismatches.")]
    public class CompareTablesCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Ignore Columns")]
        [Description("Comma seperated list of all column names that would be IGNORED for comparison in both Tables.")]
        [SampleUsage("Col1,Col2,Col3 || {vIgnoreColumns}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_IgnoreColumns { get; set; }

        [Required]
        [DisplayName("Lookup Columns")]
        [Description("Comma seperated list of all column names that would be looked up in both Tables.")]
        [SampleUsage("Col1,Col2,Col3 || {vLookupColumns}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_LookupColumns { get; set; }

        [Required]
        [DisplayName("Expected DataTable")]
        [Description("DataTable that has values that are expected.")]
        [SampleUsage("{vExpectedDT}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_Expected { get; set; }  //DataTable

        [Required]
        [DisplayName("Actual DataTable")]
        [Description("DataTable where values need to be looked up for potential mismatches.")]
        [SampleUsage("{vActualDT}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_Actual { get; set; } //DataTable

        [Required]
        [Editable(false)]
        [DisplayName("Output Differences DataTable Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_OutputUserVariableName { get; set; }
        //"Resulting output DataTable that has a report of all matches & mismatches. Columns would be all the Key columns and 
        //additionally 'Results' and 'Error' columns to communicate the result. Results would be 'Passed', 'Failed' or 'Error' 
        //and Error column would communicate details"

        public CompareTablesCommand()
        {
            CommandName = "CompareTablesCommand";
            SelectionName = "Compare Tables";
            CommandEnabled = true;
            CommandIcon = Resources.command_files;
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            var vIgnoreColumns = v_IgnoreColumns.ConvertUserVariableToString(engine);
            var vLookupColumns = v_LookupColumns.ConvertUserVariableToString(engine);
            var vExpected = (DataTable)v_Expected.ConvertUserVariableToObject(engine, nameof(v_Expected), this);
            var vActual = (DataTable)v_Actual.ConvertUserVariableToObject(engine, nameof(v_Actual), this);

            //Unnecessary with validation
            //if (vExpected == null)
               // throw new ArgumentNullException("Expected Table cannot be null");

           // if (vActual == null)
               // throw new ArgumentNullException("Actual Table cannot be null");

           // if(string.IsNullOrEmpty(vLookupColumns))
                //throw new ArgumentNullException("LookupColumns cannot be empty");


            TableComparisonManager tcm = new TableComparisonManager();
            tcm.IgnoreColumns = vIgnoreColumns;
            tcm.LookupColumns = vLookupColumns;
            tcm.Compare(vExpected, vActual);
            DataTable diff = tcm.Differences;

            diff.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IgnoreColumns", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LookupColumns", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Expected", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Actual", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Expected '{v_Expected}' - Actual '{v_Actual}' - Store Differences DataTable in '{v_OutputUserVariableName}']";
        }
    }
}
