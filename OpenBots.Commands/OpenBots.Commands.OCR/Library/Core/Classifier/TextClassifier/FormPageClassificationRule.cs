using System.Collections.Generic;

namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{

    public class FormPageClassificationRule
    {
        public FormPageClassificationRule()
        {
            IdentifyingFields = new List<MappingIdentifyingfField>();
            OtherFields = new List<string>();
        }

      
        internal string ID { get; set; }


        //SchemaID
        public string FormID { get; set; }


        //Schema Name
        public string FormName { get; set; }

        public string FormEdition { get; set; }

        // PageNumber of the Schema
        public int? PageNumber { get; set; }

        public List<MappingIdentifyingfField> IdentifyingFields { get; set; }
        public List<string> OtherFields { get; set; }
        public string HuMoments { get; set; }
    }

    public struct MappingIdentifyingfField
    {
        public string FieldText;
        public float Weight;
    };
}
