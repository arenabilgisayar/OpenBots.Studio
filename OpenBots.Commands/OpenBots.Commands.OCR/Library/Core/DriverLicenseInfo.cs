using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TextXtractor.Ocr.Core
{
    public class DrivingLicenseInfo
    {
        [JsonProperty("type")]
        public string LicenseType { get; set; }
        [JsonProperty("values")]
        public LicenseDetails Details { get; set; }
        [JsonProperty("confidence_score")]
        public double ConfidenceScore { get; set; }
    }

    public class LicenseDetails
    {
        public List<string> Address { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime IssueDate { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
    }

}