using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;
using System.Linq;

namespace TextXtractor.Ocr.GoogleCloud
{
    public class ConvertResponse
    {
        public OcrResult FromAnnotation(TextAnnotation annotation)
        {

        
            OcrResult result = FromPage(annotation.Pages[0]);
            result.Text = annotation.Text;
            


            return result;
        }

        public OcrResult FromPage(Page page)
        {
            OcrResult result = new OcrResult();
            result.Confidence = page.Confidence;

            var language = page.Property.DetectedLanguages.ToList().OrderByDescending(l => l.Confidence).FirstOrDefault();
            if(language != null)
                result.Language = new ValueConfidence<string>(language.LanguageCode, language.Confidence);

            int textPosition = 0;
            result.Height = page.Height;
            result.Width = page.Width;
            int ordinal = 0;
            foreach (var block in page.Blocks)
            {
                var blockBox = FromBlock(block,ref textPosition);
                blockBox.Ordinal = ordinal++;
                result.Boxes.Add(blockBox);
            }

            return result;
        }


        public BoundingBox FromBlock(Block block, ref int textPosition)
        {
            BoundingBox blockBox = FromBoundingPoly(block.BoundingBox);
            blockBox.Confidence = block.Confidence;
            blockBox.Type = BoundingBox.BOXTYPE_BLOCK;
            int ordinal = 0;
            foreach (var para in block.Paragraphs)
            {
                var paraBox = FromParagraph(para);
                var lines = SplitParagraphToLines(paraBox);

                foreach (var line in lines)
                {
                    line.Ordinal = ordinal++;
                    line.TextPosition = textPosition;
                    textPosition += line.Text.Length;
                }
                blockBox.Boxes.AddRange(lines);
            }

            return blockBox;
        }


        public BoundingBox FromParagraph(Paragraph para)
        {
            BoundingBox paraBox = FromBoundingPoly(para.BoundingBox);
            paraBox.Type = BoundingBox.BOXTYPE_LINE;
            paraBox.Confidence = para.Confidence;
            int ordinal = 0;
            foreach (var word in para.Words)
            {
                var wordBox = FromWord(word);
                wordBox.Ordinal = ordinal++;
                paraBox.Boxes.Add(wordBox);
                if (!string.IsNullOrEmpty(wordBox.Text))
                    paraBox.Text += wordBox.Text;
            }
            //paraBox.Text = paraBox.Text.Trim();
            return paraBox;
        }

        public List<BoundingBox> SplitParagraphToLines(BoundingBox paraBox)
        {
            List<BoundingBox> Lines = new List<BoundingBox>();
            if(!paraBox.Text.Contains("\n"))
            {
                paraBox.Type = BoundingBox.BOXTYPE_LINE;
                Lines.Add(paraBox);
                return Lines;
            }
            Dictionary<int, List<BoundingBox>> groupedBoxes = new Dictionary<int, List<BoundingBox>>();
            int iterator = 0;
            foreach (BoundingBox wordBox in paraBox.Boxes)
            {
                List<BoundingBox> group;

                if (!groupedBoxes.ContainsKey(iterator))
                {
                    group = new List<BoundingBox>();
                    groupedBoxes.Add(iterator, group);
                }
                group = groupedBoxes[iterator];
                group.Add(wordBox);
                groupedBoxes[iterator] = group;
                if (wordBox.Text.EndsWith("\n"))
                    iterator++;
            }

            foreach(var wordKvp in groupedBoxes)
            {
                iterator = wordKvp.Key;
                List<BoundingBox> group = wordKvp.Value;
              
                BoundingBox firstWordbox = group.OrderBy(b => b.Top).ThenBy(b => b.Left).FirstOrDefault();
                BoundingBox lastWordbox = group.OrderBy(b => b.Top).ThenBy(b => b.Left).LastOrDefault();

                BoundingBox lineBox = new BoundingBox(firstWordbox.TopLeft, lastWordbox.BottomRight);
                lineBox.Ordinal = iterator;
                lineBox.Boxes = group;
                lineBox.Confidence = paraBox.Confidence;
                lineBox.Type = BoundingBox.BOXTYPE_LINE;

                foreach(var wordBox in group.OrderBy( b => b.Ordinal))
                {
                    lineBox.Text += wordBox.Text;
                }
                
                Lines.Add(lineBox);
            }



            return Lines;
        }


        public BoundingBox FromWord(Word word)
        {
            BoundingBox wordBox = FromBoundingPoly(word.BoundingBox);
            wordBox.Type = BoundingBox.BOXTYPE_WORD;
            wordBox.Confidence = word.Confidence;
            int ordinal = 0;
            foreach(var sym in word.Symbols)
            {
                var symbolBox = FromSymbol(sym);
                symbolBox.Ordinal = ordinal++;
                wordBox.Boxes.Add(symbolBox);
                if (!string.IsNullOrEmpty(symbolBox.Text))
                    wordBox.Text += symbolBox.Text;

                if (sym.Property != null && sym.Property.DetectedBreak != null)
                {
                    if (sym.Property.DetectedBreak.Type == TextAnnotation.Types.DetectedBreak.Types.BreakType.Space
                        || sym.Property.DetectedBreak.Type == TextAnnotation.Types.DetectedBreak.Types.BreakType.SureSpace)
                        wordBox.Text += " ";

                    if (sym.Property.DetectedBreak.Type == TextAnnotation.Types.DetectedBreak.Types.BreakType.LineBreak
                        || sym.Property.DetectedBreak.Type == TextAnnotation.Types.DetectedBreak.Types.BreakType.EolSureSpace)
                        wordBox.Text += "\n";

                    if (sym.Property.DetectedBreak.Type == TextAnnotation.Types.DetectedBreak.Types.BreakType.Hyphen)
                        wordBox.Text += "-";
                }
            }
           // wordBox.Text = wordBox.Text.Trim();
            return wordBox;
        }

        public BoundingBox FromSymbol(Symbol symbol)
        {
            BoundingBox symbolBox = FromBoundingPoly(symbol.BoundingBox);
            symbolBox.Text = symbol.Text;
            symbolBox.Type = BoundingBox.BOXTYPE_SYMBOL;
      
            symbolBox.Confidence = symbol.Confidence;

            return symbolBox;
        }

        public BoundingBox FromBoundingPoly(BoundingPoly bbox)
        {
            return new BoundingBox(bbox.Vertices.ToArray().ToPoints());
        }

    }
}
