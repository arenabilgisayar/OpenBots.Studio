using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace OpenBots.Commands.Documents.Models
{
    [GeneratedCode("NJsonSchema", "10.1.11.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class ExtractedDocumentView
    {
        [JsonProperty("order", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Order { get; set; }

        [JsonProperty("tenantId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? TenantId { get; set; }

        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("numberOfPages", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public int? NumberOfPages { get; set; }

        [JsonProperty("qualityScore", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public double? QualityScore { get; set; }

        [JsonProperty("pageRangeLabel", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string PageRangeLabel { get; set; }

        [JsonProperty("isVerified", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVerified { get; set; }

        [JsonProperty("hasErrors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasErrors { get; set; }

        [JsonProperty("documentClassificationType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentClassificationType { get; set; }

        [JsonProperty("isExtractedContentStructured", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsExtractedContentStructured { get; set; }

        [JsonProperty("schema", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Schema { get; set; }

        [JsonProperty("sessionID", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.Guid? SessionID { get; set; }

        [JsonProperty("extractedFileId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.Guid? ExtractedFileId { get; set; }

        [JsonProperty("organizationUnitId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public long? OrganizationUnitId { get; set; }

        [JsonProperty("documentId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.Guid? DocumentId { get; set; }

        [JsonProperty("isReadOnly", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsReadOnly { get; set; }

        [JsonProperty("version", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Version { get; set; }

        [JsonProperty("entityId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public System.Guid? EntityId { get; set; }

        [JsonProperty("pages", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<PageNumber> Pages { get; set; }

        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

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

        public static ExtractedDocumentView FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ExtractedDocumentView>(data);
        }
    }
}
