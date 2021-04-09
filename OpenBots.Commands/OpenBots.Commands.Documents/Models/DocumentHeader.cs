using System.Data;

namespace OpenBots.Commands.Documents.Models
{
    public class DocumentHeader
    {
        public int DocumentNumber { get; set; }
        public string Schema { get; set; }
        public string PageNumbers { get; set; }
        public string Folder { get; set; }
        public string DocumentId { get; set; }
        public double Confidence { get; set; }

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

        public void AddRow(DataTable table)
        {
            DataRow row = table.NewRow();
            row["DocumentNumber"] = DocumentNumber;
            row["Schema"] = Schema;
            row["PageNumbers"] = PageNumbers;
            row["Folder"] = Folder;
            row["DocumentId"] = DocumentId;
            row["Confidence"] = Confidence;
            table.Rows.Add(row);
        }
    }
}
