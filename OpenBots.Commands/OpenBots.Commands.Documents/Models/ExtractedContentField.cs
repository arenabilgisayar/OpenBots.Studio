using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace OpenBots.Commands.Documents.Models
{
    public class ExtractedContentField
    {
        public decimal confidence { get; set; }
        public double top { get; set; }
        public double left { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public int page { get; set; }
        public string value { get; set; }

        public static Dictionary<string, ExtractedContentField> Parse(string jsonExtractedContent)
        {

            if(!string.IsNullOrEmpty(jsonExtractedContent) && jsonExtractedContent.StartsWith("\"") && jsonExtractedContent.EndsWith("\""))
                jsonExtractedContent = JsonConvert.SerializeObject(JsonConvert.DeserializeObject<object>(jsonExtractedContent));

            JObject o = JObject.Parse(jsonExtractedContent);
            var eCon = o["ExtractedContent"];
            var eDocs = eCon.Children().First();
            var eDoc = eDocs["document"];
            var fields = eDoc.ToObject<Dictionary<string, ExtractedContentField>>();
            return fields;
        }
    }
}
