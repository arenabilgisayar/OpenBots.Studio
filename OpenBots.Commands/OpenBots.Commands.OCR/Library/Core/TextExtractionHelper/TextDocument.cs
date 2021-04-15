using System;
using System.Collections.Generic;
using System.Drawing;

namespace TextXtractor.Ocr.Core.TextExtractionHelper
{
    public class TextDocument
    {
        public string Name { get; set; }

        public List<Text> Content { get; set; }

        public Text Text { get; set; }

        public Text Capture(Rectangle rect, string prefix = "") { throw new NotImplementedException(); }
        public Text Capture(int left, int top, int width, int height, string prefix = "")
        {
            return Capture(new Rectangle(left, top, width, height), prefix);
        }

        public static TextDocument FromFile(string filePath, string name = "")
        {
            string text = System.IO.File.ReadAllText(filePath);
            return new TextDocument()
            {
                Name = name,
                Text = new Text(text, null)
            };
        }
    }
}
