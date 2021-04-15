namespace TextXtractor.Ocr.Core
{
    public interface ISettings
    {
        string GetValue(string primaryKey, string fallbackKey = "");
    }
}