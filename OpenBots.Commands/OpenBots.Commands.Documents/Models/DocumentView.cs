using System;
using System.Collections.Generic;
using System.Text;

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
