using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    /// <summary>Response of Submitting a Document to Openbots Documents</summary>
    [GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SubmitDocumentResponse
    {
        /// <summary>ID of the HumanTask that was created as a result of this Request.</summary>
        [JsonProperty("humanTaskID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string humanTaskID { get; set; }

        /// <summary>Internal Session ID of the IDP Engine</summary>
        [JsonProperty("sessionID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string sessionID { get; set; }

        /// <summary>Any debug messages if any.</summary>
        [JsonProperty("message", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

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

        public static SubmitDocumentResponse FromJson(string data)
        {
            return JsonConvert.DeserializeObject<SubmitDocumentResponse>(data);
        }
    }
}
