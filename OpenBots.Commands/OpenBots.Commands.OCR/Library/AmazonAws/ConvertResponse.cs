using Amazon.Textract.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;
using System.Linq;
using System.IO;

namespace TextXtractor.Ocr.AmazonAws
{
    public class ConvertResponse
    {
        DetectDocumentTextResponse response;
        protected List<string> processedId;
        private Stream imageSteam;

        protected int width;
        protected int height;

        public ConvertResponse(Stream imageSteam)
        {
            this.imageSteam = imageSteam;
        }

        public OcrResult FromDetectDocumentTextResponse(DetectDocumentTextResponse response)
        {
            this.response = response;
            OcrResult result = new OcrResult();
            processedId = new List<string>();

            var image = System.Drawing.Image.FromStream(imageSteam);
            width = image.Width;
            height = image.Height;

            result.Height = height;
            result.Width = width;

            List<Core.BoundingBox> boxes = new List<Core.BoundingBox>();
            foreach (var block in this.response.Blocks)
            {
                if(!processedId.Contains(block.Id) )
                    boxes.Add(FromBlock(block));
            }

            var pageBox = boxes.FirstOrDefault();
            if(pageBox != null && pageBox.Type == "PAGE")
            {
                result.Boxes = pageBox.Boxes;
            }
            else
                result.Boxes = boxes;

            return result;
        }

        public Core.BoundingBox FromBlock(Block block)
        {
            var boxPolygon = block.Geometry.Polygon;
            //convert polygon to actual cordinates
            int x1 = (int)(boxPolygon[0].X * width);
            int x2 = (int)(boxPolygon[1].X * width);
            int x3 = (int)(boxPolygon[2].X * width);
            int x4 = (int)(boxPolygon[3].X * width);

            int y1 = (int)(boxPolygon[0].Y * height);
            int y2 = (int)(boxPolygon[1].Y * height);
            int y3 = (int)(boxPolygon[2].Y * height);
            int y4 = (int)(boxPolygon[3].Y * height);

            Core.BoundingBox box = new Core.BoundingBox(x1, y1, x2, y2, x3, y3, x4, y4);
            box.Type = block.BlockType.Value;
            box.Text = block.Text;
            box.Confidence = block.Confidence;
            processedId.Add(block.Id);
            foreach (var relatioship in block.Relationships)
            {
                if (relatioship.Type.Value == "CHILD")
                {
                    foreach (var id in relatioship.Ids)
                    {
                        foreach (var childBlock in response.Blocks.Where(b => b.Id == id))
                        {
                            var childBox = FromBlock(childBlock);
                            box.Boxes.Add(childBox);
                        }
                    }
                }
            }
            return box;
        }

        public string ExtractText(OcrResult result)
        {
            string text = string.Empty;
            foreach(var box in result.Boxes)
            {
                if(box.Type == "LINE")
                {
                    text = text + Environment.NewLine + box.Text;
                }
            }
            return text;
        }

    }
}
