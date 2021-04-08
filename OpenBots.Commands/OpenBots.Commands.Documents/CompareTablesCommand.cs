using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Category("OpenBots Documents")]
    [Description("Compare two tables cell by cell and reports any mismatches")]
    public class CompareTablesCommand : ScriptCommand
    {
        [Required]
        [Category("Input")]
        [DisplayName("Ignore Columns")]
        [Description("Comma seperated list of all column names that would be IGNORED for comparison in both Tables.")]

        public string v_IgnoreColumns { get; set; }

        [Required]
        [Category("Input")]
        [DisplayName("Lookup Columns")]
        [Description("Comma seperated list of all column names that would be looked up in both Tables.")]
        public string v_LookupColumns { get; set; }

        [Required]
        [Category("Input")]
        [DisplayName("Expected")]
        [Description("DataTable that has values that are expected.")]

        public string v_Expected { get; set; }  //DataTable

        [Required]
        [Category("Input")]
        [DisplayName("Actual")]
        [Description("DataTable where values need to be looked up for potential mismatches.")]

        public string v_Actual { get; set; } //DataTable

        [Required]
        [Category("Output")]
        [DisplayName("Differences")]
        [Description("Resulting output DataTable that has a report of all matches & mismatches. Columns would be all the Key columns and additionally 'Results' and 'Error' columns to communicate the result. Results would be 'Passed', 'Failed' or 'Error' and Error column would communicate details")]
        public string v_Differences { get; set; } //DataTable

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

            diff.StoreInUserVariable(engine, v_Differences, nameof(v_Differences), this);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" []";
        }
    }
}
