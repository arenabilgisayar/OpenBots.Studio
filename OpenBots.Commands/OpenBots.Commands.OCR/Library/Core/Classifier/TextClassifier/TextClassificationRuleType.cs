namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{
    public enum TextClassificationRuleType : int
    {
        Equals = 0,
        ContainsText = 1,
        NotContainText = 2,
        ContainsRegEx = 3,
        NotContainRegEx = 4,
        ContainsSimilarText = 5
    }
}
