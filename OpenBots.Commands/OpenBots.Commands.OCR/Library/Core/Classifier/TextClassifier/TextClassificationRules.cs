using System.Collections.Generic;

namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{
    public class TextClassificationRules : List<TextClassificationRule>
    {
        public TextClassificationRules()
        {
        }

        public TextClassificationRules(IEnumerable<TextClassificationRule> collection) : base(collection)
        {
        }

        public TextClassificationRules(string name)
        {
            Name = name;
        }
        public TextClassificationRules(string formName, List<MappingIdentifyingfField> indentifyingFields, List<string> labels) : this(formName, indentifyingFields.ToArray(), labels.ToArray())
        { Name = formName; }

        public TextClassificationRules(string formName, MappingIdentifyingfField[] indentifyingFields, string[] labels) : this(formName, indentifyingFields)
        {
           
            foreach (string label in labels)
            {
                if (!string.IsNullOrEmpty(label))
                {
                    TextClassificationRule rule = new TextClassificationRule();
                    rule.RuleType = TextClassificationRuleType.ContainsText;
                    rule.Pattern = label;
                    rule.Weight = 1;
                    this.Add(rule);
                }
            }
        }

        public TextClassificationRules(string formName, MappingIdentifyingfField[] indentifyingFields)
        {
            Name = formName;
            foreach(MappingIdentifyingfField identifyingField in indentifyingFields)
            {
                if (!string.IsNullOrEmpty(identifyingField.FieldText))
                {
                    TextClassificationRule rule = new TextClassificationRule();
                    rule.RuleType = TextClassificationRuleType.ContainsText;
                    rule.Pattern = identifyingField.FieldText;
                    rule.Weight = identifyingField.Weight;
                    this.Add(rule);
                }
            }
        }


        public string Name { get; set; }
    }
}
