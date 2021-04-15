using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.MicrosoftAzure
{
    public class ConvertResponse
    {

        public Core.OcrResult FromTextRecognitionResult(TextRecognitionResult recognitionResult)
        {
            Core.OcrResult result = new Core.OcrResult();

            result.Width = recognitionResult.Width ;
            result.Height = recognitionResult.Height;
            result.Orientation = recognitionResult.ClockwiseOrientation;
            int lineNumber = 0;
            int textPosition = 0;
            foreach(var line in recognitionResult.Lines)
            {
                var lineBox = FromLine(line);
                lineBox.Ordinal = lineNumber++;
                lineBox.TextPosition = textPosition;
                textPosition += lineBox.Text.Length;
                result.Boxes.Add(lineBox);
            }
            
            return result;
        }

        public BoundingBox FromLine(Line line)
        {
            BoundingBox lineBox = new BoundingBox(line.BoundingBox) ;
            lineBox.Type = BoundingBox.BOXTYPE_LINE;
            lineBox.Text = line.Text;
            int ordinal = 0;
            foreach(var word in line.Words)
            {
                var wordBox = FromWord(word);
                wordBox.Ordinal = ordinal++;
                lineBox.Boxes.Add(wordBox);
            }
            return lineBox;

            
        }

        public BoundingBox FromWord(Word word)
        {
            BoundingBox wordBox = new BoundingBox(word.BoundingBox);
            wordBox.Type = BoundingBox.BOXTYPE_WORD;
            wordBox.Text = word.Text;
            if (word.Confidence.HasValue)
            {
                if (word.Confidence.Value == TextRecognitionResultConfidenceClass.High)
                    wordBox.Confidence = 80;
                if (word.Confidence.Value == TextRecognitionResultConfidenceClass.Low)
                    wordBox.Confidence = 30;
                else
                    wordBox.Confidence = 50;
            }

            return wordBox;
        }





    }
}
