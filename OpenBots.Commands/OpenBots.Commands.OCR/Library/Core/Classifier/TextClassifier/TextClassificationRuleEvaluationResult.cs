using System;

namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{
    public class TextClassificationRuleEvaluationResult
    {
        public TextClassificationRuleEvaluationResult()
        {
        }

        public TextClassificationRuleEvaluationResult(string name)
        {
            Name = name;
        }

        public TextClassificationRuleEvaluationResult(string name, int weight) : this(name)
        {
            Weight = weight;
        }

   

        public string Name { get; set; }

        public float Weight { get; set; }

        public float TotalWeight { get; set; }

        public int Passed { get; set; }

        public int Failed { get; set; }

        public float AverageWeight
        {
            get
            {
                if (Weight <= 0) return 0;
                else return Weight / TotalWeight ;
            }
        }

        public string Text { get; internal set; }
    }
}
