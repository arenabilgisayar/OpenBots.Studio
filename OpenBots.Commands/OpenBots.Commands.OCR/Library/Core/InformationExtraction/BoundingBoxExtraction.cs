using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class BoundingBoxExtraction
    {
        OcrResult _ocrResult;

        public BoundingBoxExtraction(OcrResult ocrResult)
        {
            _ocrResult = ocrResult ?? throw new ArgumentNullException(nameof(ocrResult));
        }

        public IEnumerable<ExtractionValue> Read(ExtractionField field)
        {
            var boxes = DeepFindBox(_ocrResult.Boxes, 
                b => b.Type == BoundingBox.BOXTYPE_LINE 
                && field.Box.HasValue 
                && b.IsInside(field.Box.Value)
                // If a Confidence Criteria has been defined, then make sure the box has confidence above the cutoff
                && (field.ConfidenceCutoff.HasValue && b.Confidence > 0 && b.Confidence > field.ConfidenceCutoff) );

            foreach(var box in boxes)
            {
                ExtractionValue exvalue = new ExtractionValue(box);
                exvalue.PageNumber = field.PageNumber;
                exvalue.Name = field.Name;
                if (!string.IsNullOrEmpty(field.PrefixLabel) && exvalue.Value.StartsWith(field.PrefixLabel))
                    exvalue.Value = exvalue.Value.Substring(field.PrefixLabel.Length);
                yield return exvalue;
            }
        
            yield return null;
        }

        public ExtractionValue Join(List<ExtractionValue> values)
        {
            return null;
        }

        public string CreateJsonString(IEnumerable<ExtractionValue> values)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                foreach (ExtractionValue value in values)
                {
                    writer.WritePropertyName(value.Name);
                    writer.WriteValue(value.Value);
                    writer.WritePropertyName(string.Format("{0}_Confidence", value.Name));
                    writer.WriteValue(value.Confidence);
                }

                writer.WriteEndObject();
            }

           return sb.ToString();
        }



        public IEnumerable<BoundingBox> DeepFindBox(List<BoundingBox> boxes,  Predicate<BoundingBox> predicate)
        {
            List<BoundingBox> boundingBoxes = new List<BoundingBox>();
            foreach(BoundingBox box in boxes)
            {
                if (predicate.Invoke(box))
                    boundingBoxes.Add(box);

                boundingBoxes.AddRange(DeepFindBox(box.Boxes, predicate));
            }
            return boundingBoxes;
        }
  
    }

    public enum FieldType : int
    {
        Unknown = 0,
        Value= 1 ,
        Label = 2,
        Exclusion = 3
    }

    public enum RecognitionType : int
    {
        Unknown = 0,
        Text = 1,
        Checkbox =2,
        Date = 3,
        Currency =4,
        Singlechar = 5,
        Barcode = 6,
        Qrcode = 7,
        Mrz = 8,
        Address = 9,
        Other = 10
    }


}
