using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Commands.Documents.Models
{
    /// <summary>Response of Submitting a Document to Openbots Documents</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class SubmitDocumentResponse
    {
        /// <summary>ID of the HumanTask that was created as a result of this Request.</summary>
        [Newtonsoft.Json.JsonProperty("humanTaskID", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string humanTaskID { get; set; }

        /// <summary>Internal Session ID of the IDP Engine</summary>
        [Newtonsoft.Json.JsonProperty("sessionID", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string sessionID { get; set; }

        /// <summary>Any debug messages if any.</summary>
        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static SubmitDocumentResponse FromJson(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitDocumentResponse>(data);
        }

    }
}
