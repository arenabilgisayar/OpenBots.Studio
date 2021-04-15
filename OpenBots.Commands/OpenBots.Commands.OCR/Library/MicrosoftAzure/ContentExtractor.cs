using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.MicrosoftAzure
{
    public class ContentExtractor
    {

       public string GetExtractedText(Core.OcrResult extractedContent)
        {
            //Arranging words using updated line number then by their left value and finally combining all the words using space as seperator.
            string extractedText = string.Join(" ", GetExtractedWords(extractedContent).Select(s => s.Text).ToArray());

            return extractedText;
        }

        public string GetExtractedTextWithSpaces(Core.OcrResult extractedContent)
        {
            var groupedWordsByLineNumber = GetExtractedWords(extractedContent).GroupBy(word => word.Ordinal);

            var wordWidthPerCharacterList = groupedWordsByLineNumber.SelectMany(s => s.Where(w => w.Text.Length > 0).Select(w => (double)w.Width / w.Text.Length));

            var avgWidthPerCharacterInText = wordWidthPerCharacterList.Average();
            var min = (wordWidthPerCharacterList.Min() + avgWidthPerCharacterInText) / 2;
            var max = (wordWidthPerCharacterList.Max() + avgWidthPerCharacterInText) / 2;
            avgWidthPerCharacterInText = wordWidthPerCharacterList.Where(width => width < avgWidthPerCharacterInText + max && width > avgWidthPerCharacterInText - min).Average();

            string extractedText = string.Join(Environment.NewLine,
                groupedWordsByLineNumber
                    .Select(words => JoinWords(words, avgWidthPerCharacterInText))
                    .ToList()
                );

            return extractedText;
        }


        private string JoinWords(IGrouping<int, BoundingBox> words, double avgWidthPerCharacterInLine)
        {
            var joinedWords = string.Empty;

            var wordsList = words.ToList();
            var wordsCount = wordsList.Count();
            if (wordsCount > 0)
            {


                joinedWords += new String(' ', GetSpaceLegth(null, wordsList[0], avgWidthPerCharacterInLine));
                joinedWords += wordsList[0].Text;

                for (int i = 1; i < wordsCount; i++)
                {
                    int spacesBtwnWords = GetSpaceLegth(wordsList[i - 1], wordsList[i], avgWidthPerCharacterInLine);

                    joinedWords += new String(' ', spacesBtwnWords);
                    joinedWords += wordsList[i].Text;
                }
            }

            return joinedWords;
        }

        private int GetSpaceLegth(BoundingBox prevWord, BoundingBox word, double avgWidthPerCharacter)
        {
            var leftWidth = word.TopLeft.X;
            var leftWidthPrevWord = 0;
            if (prevWord != null)
                leftWidthPrevWord = prevWord.TopLeft.X + prevWord.Width;

            var widthBtwnWords = leftWidth - leftWidthPrevWord;
            int spacesBtwnWords = (int)Math.Ceiling((widthBtwnWords / avgWidthPerCharacter));

            return spacesBtwnWords <= 0 ? 1 : spacesBtwnWords;
        }


        /// <summary>
        /// Method that takes extracted document as input and returns extracted content as list of words.
        /// </summary>
        /// <param name="extractedContent">extractedContentt from which content to be extracted into string.</param>
        /// <returns>Extracted content as string</returns>
        public List<BoundingBox> GetExtractedWords(Core.OcrResult extractedContent)
        {

            //Looping through all the words after flattening all the lines using SelectMany.
            IEnumerable<BoundingBox> words = extractedContent.Boxes.SelectMany(line => line.Boxes);

            //Arranging words as per their top and then their left value.
            var tempSortedWords = words.OrderBy(o => o.Top).ThenBy(t => t.Left).ToList();

            //Logic to update line number in words.
            BoundingBox preWord = null;
            BoundingBox currWord = null;
            int lineNo = 1;

            int tempSortedWordsCount = tempSortedWords.Count();
            if (tempSortedWordsCount > 0)
            {
                tempSortedWords[0].Ordinal = lineNo;

                for (int i = 1; i < tempSortedWordsCount; i++)
                {
                    preWord = tempSortedWords[i - 1];
                    currWord = tempSortedWords[i];

                    //If difference between current word top and previous world top is greater than 5 then consider that line is changing.
                    if (currWord.Top > preWord.Top + 5) // if true line changing...
                        tempSortedWords[i].Ordinal = ++lineNo;
                    else
                        tempSortedWords[i].Ordinal = lineNo;
                }
            }
            //Arranging words using updated line number then by their left value and finally combining all the words using space as seperator.
            return tempSortedWords.OrderBy(o => o.Ordinal).ThenBy(t => t.Left).ToList();

        }
    }
}
