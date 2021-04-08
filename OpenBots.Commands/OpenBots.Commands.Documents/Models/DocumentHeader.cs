using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Models
{
    public class DocumentInfo
    {
        public DocumentInfo()
        {
            Documents = new List<DocumentHeader>();
        }

        public string TaskId { get; set; }
        public List<DocumentHeader> Documents { get; set; }

        public void Add(int documentNumber, string schema = "", string pageNumbers = "", string folder = "", string documentId = "")
        {
            Documents.Add(new DocumentHeader(documentNumber, schema, pageNumbers, folder, documentId));
        }
        public DataTable CreateDataTable()
        {
            DataTable table = new DataTable("Results");
            table.Columns.Add("DocumentNumber", typeof(int));
            table.Columns.Add("Schema", typeof(string));
            table.Columns.Add("PageNumbers", typeof(string));
            table.Columns.Add("Folder", typeof(string));
            table.Columns.Add("DocumentId", typeof(string));
            table.Columns.Add("Confidence", typeof(double));

            if(Documents != null)
                foreach (DocumentHeader header in Documents)
                    if (header != null)
                        header.AddRow(table);

            return table;
        }

        public string SerializeJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class DocumentHeader
    {
        public DocumentHeader()
        {
        }

        public DocumentHeader(int documentNumber, string schema = "", string pageNumbers = "", string folder = "", string documentId = "")
        {
            DocumentNumber = documentNumber;
            Schema = schema;
            PageNumbers = pageNumbers;
            Folder = folder;
            DocumentId = documentId;
            Confidence = 0.9;
        }

        public int DocumentNumber { get; set; }
        public string Schema { get; set; }
        public string PageNumbers { get; set; }

        public string Folder { get; set; }

        public string DocumentId { get; set; }

        public double Confidence { get; set; }

   

        public void AddRow(DataTable table)
        {
            DataRow row = table.NewRow();
            row["DocumentNumber"] = this.DocumentNumber;
            row["Schema"] = this.Schema;
            row["PageNumbers"] = this.PageNumbers;
            row["Folder"] = this.Folder;
            row["DocumentId"] = this.DocumentId;
            row["Confidence"] = this.Confidence;
            table.Rows.Add(row);
        }

    }
}
