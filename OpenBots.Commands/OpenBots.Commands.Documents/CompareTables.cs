using OpenBots.Commands.Documents.Library;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Compare Tables")]
    [Description("Compare two tables cell by cell and reports any mismatches")]
    public class CompareTables : ScriptCommand
    {
        [Category("Input")]
        [DisplayName("Ignore Columns")]
        [Required]
        [Description("Comma seperated list of all column names that would be IGNORED for comparison in both Tables.")]

        public string v_IgnoreColumns { get; set; }

        [Category("Input")]
        [DisplayName("Lookup Columns")]
        [Required]
        [Description("Comma seperated list of all column names that would be looked up in both Tables.")]
        public string v_LookupColumns { get; set; }

        [Category("Input")]
        [DisplayName("Expected")]
        [Required]
        [Description("DataTable that has values that are expected.")]

        public string v_Expected { get; set; }  //DataTable

        [Category("Input")]
        [DisplayName("Actual")]
        [Required]
        [Description("DataTable where values need to be looked up for potential mismatches.")]

        public string v_Actual { get; set; } //DataTable

        [Category("Output")]
        [DisplayName("Differences")]
        [Required]
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
    }
}
