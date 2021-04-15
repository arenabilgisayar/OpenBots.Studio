using System;
using System.Text;

namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{

    public class TextClassificationRule
    {
        public TextClassificationRuleType RuleType { get; set; }

        public string Pattern { get; set; }

        public int Distance { get; set; }

        public float Weight { get; set; }
    }
}
