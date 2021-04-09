using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    public class ExtractedContentContainer
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, ExtractedContentField>>> ExtractedContent { get; set; }
    }
}
