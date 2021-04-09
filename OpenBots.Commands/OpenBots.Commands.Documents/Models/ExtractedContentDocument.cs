using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    public class ExtractedContentDocument
    {
        public Dictionary<string, ExtractedContentField> fields { get; set; }
    }
}
