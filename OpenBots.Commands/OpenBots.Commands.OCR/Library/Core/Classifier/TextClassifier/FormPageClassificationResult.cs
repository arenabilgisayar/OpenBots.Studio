namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{
    public class FormPageClassificationResult
    {
        public FormPageClassificationResult()
        {
       
        }

        public FormPageClassificationResult(FormPageClassificationRule rule)
        {
            ID = rule.ID;
            FormID = rule.FormID;
            FormName = rule.FormName;
            FormEdition = rule.FormEdition;
            PageNumber = rule.PageNumber;
            HuMoments = rule.HuMoments;
        }

        public string ID { get; set; }

        //SchemaID
        public string FormID { get; set; }

        public string FormName { get; set; }

        public string FormEdition { get; set; }

        public int? PageNumber { get; set; }

        public float? Confidence { get; set; }
        public string HuMoments { get; set; }

    }
}
