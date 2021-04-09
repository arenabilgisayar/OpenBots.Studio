using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    [GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class PageNumber
    {
        [JsonProperty("document", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Document { get; set; }

        [JsonProperty("file", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? File { get; set; }

        private IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PageNumber FromJson(string data)
        {
            return JsonConvert.DeserializeObject<PageNumber>(data);
        }
    }
}
