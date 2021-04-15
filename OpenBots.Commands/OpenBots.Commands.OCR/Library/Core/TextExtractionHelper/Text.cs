using System;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace TextXtractor.Ocr.Core.TextExtractionHelper
{

    public class Text : IText
    {

        protected TextDocument containingDocument;

        protected string innerString;

        protected DataType innerDataType;
        public Rectangle? box;

        public string Name { get; set; }

        public int? Confidence { get; set; }

        public Text()
        {
            innerDataType = DataType.String;
        }

        public Text(string innerString, Rectangle? box) : this()
        {
            this.innerString = innerString;
            this.box = box;
        }

        public static Text New(string text)
        {
            return new Text(text, null);
        }

        public static Text New(string text, Rectangle box)
        {
            return new Text(text, box);
        }

        public Text Trim()
        {
            return new Text(innerString.Trim(), box);
        }

        public Text Between(string left, string right)
        {
            string betweenString = string.Empty;
            if (innerString.IndexOf(left, StringComparison.OrdinalIgnoreCase) > 0 && innerString.IndexOf(right, StringComparison.OrdinalIgnoreCase) > 0)
            {
                int Start, End;
                Start = innerString.IndexOf(left, 0, StringComparison.OrdinalIgnoreCase) + left.Length;
                End = innerString.IndexOf(right, Start, StringComparison.OrdinalIgnoreCase);
                betweenString = innerString.Substring(Start, End - Start);
            }

            return new Text(betweenString, box);
        }

        public Text DateAfter(string left)
        {
            throw new NotImplementedException();
        }

        public Text DateBefore(string left)
        {
            throw new NotImplementedException();
        }

        public Text Before(string right)
        {
            string betweenString = innerString;
            if (innerString.IndexOf(right, StringComparison.OrdinalIgnoreCase) > 0)
            {
                int End;
                End = innerString.IndexOf(right, 0, StringComparison.OrdinalIgnoreCase);
                betweenString = innerString.Substring(0, End);
            }

            return new Text(betweenString, box);
        }

        public Text After(string left)
        {
            string betweenString = innerString;
            if (innerString.IndexOf(left, StringComparison.OrdinalIgnoreCase) > 0)
            {
                int Start;
                Start = innerString.IndexOf(left, 0, StringComparison.OrdinalIgnoreCase) + left.Length;
                betweenString = innerString.Substring(Start);
            }

            return new Text(betweenString, box);
        }

        public Text Matches(string regularExpression)
        {
            throw new NotImplementedException();
        }



        public override string ToString()
        {
            return innerString;
        }

        public Text Matches(string regularExpression, int captureGroup = 0)
        {
            throw new NotImplementedException();
        }

        public Text Clean()
        {
            // The set of Unicode character categories containing non-rendering,
            // unknown, or incomplete characters.
            // !! Unicode.Format and Unicode.PrivateUse can NOT be included in
            // !! this set, because they may (private-use) or do (format)
            // !! contain at least *some* rendering characters.
            var nonRenderingCategories = new UnicodeCategory[] {
                UnicodeCategory.Control,
                UnicodeCategory.OtherNotAssigned,
                UnicodeCategory.Surrogate,
                UnicodeCategory.LineSeparator,
                UnicodeCategory.ParagraphSeparator};

            // Char.IsWhiteSpace() includes the ASCII whitespace characters that
            // are categorized as control characters. Any other character is
            // printable, unless it falls into the non-rendering categories.

            string cleanText = innerString
                 .Select(c => Char.IsWhiteSpace(c) || !nonRenderingCategories.Contains(Char.GetUnicodeCategory(c)) || !Char.IsControl(c))
                 .Aggregate("", (current, next) => string.Concat(current, next));

            return new Text(cleanText, box);
        }


        public Text Remove(string find)
        {
            return new Text(innerString.Replace(find, ""), box);
        }

        public Text Substitute(string find, string replace)
        {
            return new Text(innerString.Replace(find, replace), box);
        }

        public Text SkipLine()
        {
            throw new NotImplementedException();
        }

        //public Text FirstLine()
        //{
            
        //}

        public Text Date()
        {
            throw new NotImplementedException();
        }

        public Text DateBetween(string left, string right)
        {
            throw new NotImplementedException();
        }

        public Text Number()
        {
            throw new NotImplementedException();
        }

        public Text NumberAfter(string left)
        {
            throw new NotImplementedException();
        }

        public Text NumberBefore(string left)
        {
            throw new NotImplementedException();
        }

        public Text NumberBetween(string left, string right)
        {
            throw new NotImplementedException();
        }

        public Text IsEmpty(string defaultText)
        {
            if (string.IsNullOrWhiteSpace(innerString) || string.IsNullOrEmpty(innerString))
                return New(defaultText);
            else
                return this;
        }

        public bool Contains(string find)
        {
            return innerString.IndexOf(find, StringComparison.OrdinalIgnoreCase) > 0;
        }

        public Text Append(string textToAppend)
        {
            return new Text(string.Concat(innerString, textToAppend), box);
        }

        public Text Prepend(string textToAppend)
        {
            return new Text(string.Concat(textToAppend, innerString), box);
        }

        public Text Lower()
        {
            return new Text(innerString.ToLowerInvariant(), box);
        }

        public Text Upper()
        {
            return new Text(innerString.ToUpperInvariant(), box);
        }

        public Text Proper()
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return new Text(myTI.ToTitleCase(innerString), box);
        }

        public Text StartsWith(string find)
        {
            throw new NotImplementedException();
        }

        public Text EndsWith(string find)
        {
            throw new NotImplementedException();
        }

        public Text Empty()
        {
            return new Text(string.Empty, box);
        }

        public Text AppendRepeat(string toRepeat, int numberOfTimes)
        {
            return new Text(string.Concat(innerString, Enumerable.Repeat<string>(toRepeat, numberOfTimes)), box);
        }

        public Text PrependRepeat(string toRepeat, int numberOfTimes)
        {
            return new Text(string.Concat(Enumerable.Repeat<string>(toRepeat, numberOfTimes), innerString), box);
        }

        public Text Left(int numberOfChar)
        {
            return new Text(innerString.Substring(0, numberOfChar), box);
        }

        public Text Right(int numberOfChar)
        {
            return new Text(innerString.Substring(innerString.Length - numberOfChar), box);
        }

        public Text Mid(int startingFrom, int numberOfChar)
        {
            return new Text(innerString.Substring(startingFrom, numberOfChar), box);
        }

        public bool Contains()
        {
            throw new NotImplementedException();
        }
    }
}
