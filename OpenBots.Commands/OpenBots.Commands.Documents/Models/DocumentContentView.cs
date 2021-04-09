using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    [GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class DocumentContentView
    {
        [JsonProperty("contentVersion", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? ContentVersion { get; set; }

        [JsonProperty("isContentReadOnly", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsContentReadOnly { get; set; }

        [JsonProperty("content", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("isVerified", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVerified { get; set; }

        [JsonProperty("hasErrors", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasErrors { get; set; }

        [JsonProperty("schema", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Schema { get; set; }

        [JsonProperty("entityId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? EntityId { get; set; }

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

        public static DocumentContentView FromJson(string data)
        {
            return JsonConvert.DeserializeObject<DocumentContentView>(data);
        }
    }
}
