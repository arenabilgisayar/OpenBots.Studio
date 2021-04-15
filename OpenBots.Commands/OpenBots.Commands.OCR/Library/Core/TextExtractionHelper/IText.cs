namespace TextXtractor.Ocr.Core.TextExtractionHelper
{
    public interface IText
    {
        Text After(string right);
        Text Before(string left);
        Text Between(string left, string right);
        Text Matches(string regularExpression, int captureGroup = 0);
        Text Trim();

        /// <summary>
        /// Returns the character specified by the code number
        /// </summary>
        /// <returns></returns>
        Text Clean();

        Text Append(string textToAppend);

        Text Prepend(string textToAppend);

        Text Remove(string find);

        Text Substitute(string find, string replace); //Substitutes new text for old text in a text string

        // Lower   Converts text to lowercase
        Text Lower();

        // Upper  Converts text to uppercase
        Text Upper();

        Text Proper();

        Text StartsWith(string find);

        Text EndsWith(string find);

        Text Empty();

        // Rept (Repeat) Repeats text a given number of times and appends it to the Text
        Text AppendRepeat(string toRepeat, int numberOfTimes);
        Text PrependRepeat(string toRepeat, int numberOfTimes);

        Text Left(int numberOfChar);

        Text Right(int numberOfChar);

        Text Mid(int startingFrom, int numberOfChar);



        // Address - City, HouseNumber, Street, State, Zip

        Text SkipLine();

        Text Date();
        Text DateAfter(string left);
        Text DateBefore(string left);
        Text DateBetween(string left, string right);

        Text Number();
        Text NumberAfter(string left);
        Text NumberBefore(string left);
        Text NumberBetween(string left, string right);



        string ToString();

        Text IsEmpty(string defaultValue);

        bool Contains(string find);


    }
}