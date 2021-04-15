using System.Drawing;

namespace TextXtractor.Ocr.Core
{
    public class ExtractionValue
    {
        public ExtractionValue()
        {
        }
        public ExtractionValue(BoundingBox box)
        {
            Value = box.Text;
            TextPosition = box.TextPosition;
            Confidence = box.Confidence;
            Box = box.ToRectange();
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public Rectangle? Box { get; set; }

        public float? Confidence { get; set; }

        public int? TextPosition { get; set; }

        public int? PageNumber { get; set; }
    }


}
