using System.Collections.Generic;

namespace TextXtractor.Ocr.Core
{
    public class OcrResult
    {
        public OcrResult()
        {
            Boxes = new List<BoundingBox>();
            MetaData = new List<ImageMetaData>();
            Barcodes = new List<BarcodeBox>();
            MicrCodes = new List<MicrCode>();
            DrivingLicenses = new List<DrivingLicenseInfo>();
        }

        public List<BoundingBox> Boxes { get; set; }

        public List<BarcodeBox> Barcodes { get; set; }

        public List<MicrCode> MicrCodes { get; set; }
        public List<DrivingLicenseInfo> DrivingLicenses { get; set; }

        public string Text { get; set; }

        public string ServiceName { get; set; }

        public string ServiceVersion { get; set; }

        public List<Field> Fields { get; set; }

        public ValueConfidence<string> Language { get; set; }

        public double TextAngle { get; set; }
        public double? Orientation { get; set; }

        public float Confidence { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public List<ImageMetaData> MetaData { get; set; }

        public int? DetectedPageNumber { get; set; }

        public int? DetectedPageNumberOf { get; set; }



        /// <summary>
        /// This is a helper method to remove the top level Blocks and lowest level Symbols that is added by Google (only) 
        /// This will allow matching results from Amazon, Microsoft and Google OCR Results.
        /// </summary>
        /// <returns></returns>
        public List<BoundingBox> GetLinesAndWords()
        {
            List<BoundingBox> lines = new List<BoundingBox>();
            foreach (BoundingBox block in Boxes)
            {
                if(block.Type == BoundingBox.BOXTYPE_BLOCK)
                {
                    foreach (BoundingBox line in block.Boxes)
                    {
                        if (line.Type == BoundingBox.BOXTYPE_LINE)
                        {
                            lines.Add(line);

                            foreach (BoundingBox word in line.Boxes)
                            {
                                if (word.Type == BoundingBox.BOXTYPE_WORD)
                                {
                                    word.Boxes.RemoveAll(b => b.Type == BoundingBox.BOXTYPE_SYMBOL);
                                }
                            }
                        }
                    }
                }
            }
            return lines;
        }
    }
}
