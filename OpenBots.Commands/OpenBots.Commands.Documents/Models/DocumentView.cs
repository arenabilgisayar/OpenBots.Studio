using System;

namespace OpenBots.Commands.Documents.Models
{
    [Serializable]
    public class DocumentView
    {
        public Guid TaskID { get; set; }
        public ExtractedDocumentView Header { get; set; }
        public DocumentContentView Content { get; set; }
    }
}
