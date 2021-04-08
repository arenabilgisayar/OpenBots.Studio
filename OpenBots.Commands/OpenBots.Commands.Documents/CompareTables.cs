using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents
{
    [Category("Openbots Documents")]
    [DisplayName("Compare Tables")]
    [Description("Compare two tables cell by cell and reports any mismatches")]
    public class CompareTables : CodeActivity
    {
        [Category("Input")]
        [DisplayName("Ignore Columns")]
        [RequiredArgument]
        [Description("Comma seperated list of all column names that would be IGNORED for comparison in both Tables.")]

        public InArgument<string> IgnoreColumns { get; set; }

        [Category("Input")]
        [DisplayName("Lookup Columns")]
        [RequiredArgument]
        [Description("Comma seperated list of all column names that would be looked up in both Tables.")]
        public InArgument<string> LookupColumns { get; set; }

        [Category("Input")]
        [DisplayName("Expected")]
        [RequiredArgument]
        [Description("DataTable that has values that are expected.")]

        public InArgument<DataTable> Expected { get; set; }

        [Category("Input")]
        [DisplayName("Actual")]
        [RequiredArgument]
        [Description("DataTable where values need to be looked up for potential mismatches.")]

        public InArgument<DataTable> Actual { get; set; }

        [Category("Output")]
        [DisplayName("Differences")]
        [RequiredArgument]
        [Description("Resulting output DataTable that has a report of all matches & mismatches. Columns would be all the Key columns and additionally 'Results' and 'Error' columns to communicate the result. Results would be 'Passed', 'Failed' or 'Error' and Error column would communicate details")]
        public OutArgument<DataTable> Differences { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string ignoreColumns = IgnoreColumns.Get(context);
            string lookupColumns = LookupColumns.Get(context);
            DataTable expected = Expected.Get(context);
            DataTable actual = Actual.Get(context);
            if (expected == null)
                throw new ArgumentNullException("Expected Table cannot be null");

            if (actual == null)
                throw new ArgumentNullException("Actual Table cannot be null");

            if(string.IsNullOrEmpty(lookupColumns))
                throw new ArgumentNullException("LookupColumns cannot be empty");


            TableComparisonManager tcm = new TableComparisonManager();
            tcm.IgnoreColumns = ignoreColumns;
            tcm.LookupColumns = lookupColumns;
            tcm.Compare(expected, actual);
            DataTable diff = tcm.Differences;
            Differences.Set(context, diff);

        }
    }
}
