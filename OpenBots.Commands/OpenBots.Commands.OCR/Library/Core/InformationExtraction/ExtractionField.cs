using System.Drawing;

namespace TextXtractor.Ocr.Core
{
    public class ExtractionField
    {
        public string Name { get; set; }
        public Rectangle? Box { get; set; }

        public string PrefixLabel { get; set; }

        public RecognitionType TypeOfRecognition { get; set; }

        public FieldType FieldType { get; set; }

        public float? ConfidenceCutoff { get; set; }

        public int? PageNumber { get; set; }
    }


}
