using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextXtractor.Ocr.Core.Classifier.TextClassifier
{


    public class TextClassificationEngine
    {

        public async Task<FormPageClassificationResult> EvaluateFormPages(string text, List<FormPageClassificationRule> formPages)
        {
            
            List<TextClassificationRules> multipleRules = new List<TextClassificationRules>();
            foreach (var form in formPages)
            {
                form.ID = Guid.NewGuid().ToString();
                TextClassificationRules rule = new TextClassificationRules(form.ID, form.IdentifyingFields, form.OtherFields);
                
                multipleRules.Add(rule);
            }
            TextClassificationRuleEvaluationResult result = EvaluateMultipleRules(text, multipleRules)?.Result;
            if (result != null)
            {
                var rule = formPages.Where(f => f.ID.Equals(result.Name)).FirstOrDefault();
                FormPageClassificationResult ruleresult = new FormPageClassificationResult(rule);
                ruleresult.Confidence = result.AverageWeight;
                return ruleresult;
            }

            return null;
        }

        public IEnumerable<FormPageClassificationResult> ClassifyFormPage(string text, List<FormPageClassificationRule> formPages)
        {

            List<TextClassificationRules> multipleRules = new List<TextClassificationRules>();
            foreach (var form in formPages)
            {
                form.ID = Guid.NewGuid().ToString();
                TextClassificationRules rule = new TextClassificationRules(form.ID, form.IdentifyingFields, form.OtherFields);
                multipleRules.Add(rule);
            }

            var result = EvaluateMultipleRulesGetPassedRule(text, multipleRules).Result;
            IList<FormPageClassificationResult> classificationResults = null;
            if (result != null)
            {
                classificationResults = new List<FormPageClassificationResult>();
                var formIds = result.Select(s=> s.Name).ToArray();
                var rules = formPages.Where(f => formIds.Contains(f.ID));
                foreach (var iRule in rules)
                {
                    FormPageClassificationResult ruleresult = new FormPageClassificationResult(iRule);
                    classificationResults.Add(ruleresult);
                }
            }
            return classificationResults;
        }

        public async Task<List<TextClassificationRuleEvaluationResult>> EvaluateMultipleRulesGetPassedRule(string text, IEnumerable<TextClassificationRules> multipleRules)
        {
            List<TextClassificationRuleEvaluationResult> results = new List<TextClassificationRuleEvaluationResult>();
            IEnumerable<Task<TextClassificationRuleEvaluationResult>> ruleTasks = from rules in multipleRules select EvaluateRulesAsync(text, rules);
            results = new List<TextClassificationRuleEvaluationResult>(await Task.WhenAll(ruleTasks));
            var winnerRules = results?.Where(q => q.Passed >= 1).OrderByDescending(r => r.AverageWeight)?.ToList();
            return winnerRules;
        }

        //    public async Task<(string, float)> EvaluateForms(string text, IDictionary<string, List<string>> formIdentifyingFields)
        //{
        //    List<TextClassificationRules> multipleRules = new List<TextClassificationRules>();
        //    foreach (var kvp in formIdentifyingFields)
        //    {
        //        TextClassificationRules rule = new TextClassificationRules(kvp.Key, kvp.Value.ToArray());
        //        multipleRules.Add(rule);
        //    }
        //    TextClassificationRuleEvaluationResult result = await EvaluateMultipleRules(text, multipleRules);
        //    if (result != null)
        //    {

        //        return (result.Name, result.AverageWeight);
        //    }

        //    return (string.Empty, 0);
        //}


        public async Task<TextClassificationRuleEvaluationResult> EvaluateMultipleRules(string text, IEnumerable<TextClassificationRules> multipleRules)
        {
            List<TextClassificationRuleEvaluationResult> results = new List<TextClassificationRuleEvaluationResult>();
            IEnumerable<Task<TextClassificationRuleEvaluationResult>> ruleTasks =  from rules in multipleRules select EvaluateRulesAsync(text, rules);
            results = new List<TextClassificationRuleEvaluationResult>(await Task.WhenAll(ruleTasks));

            //foreach (TextClassificationRules rules in multipleRules)
            //{
            //    TextClassificationRuleEvaluationResult result = await EvaluateRulesAsync(text, rules);
            //    results.Add(result);
            //}

            var winnerRule = results?.Where(q=> q.AverageWeight > 0).OrderByDescending(r => r.AverageWeight).FirstOrDefault();
            if (winnerRule == null) return winnerRule;

            /// TODO: Need to write special exception here to Evaluate Forms that may have Conflicts like I797 and I797A, I797A Nov
            /// If there are more than one Rules that result with the same top score
            /// 
            /// Currently we will handle only if there are TWO top scorers
            /// If there are TWO conflicting Rules that have the Same Top Score, 
            /// it is quite possible that one rule is more generic while the other may be less generic / more specific
            /// In this case we want the more specific one to be the winner. 
            /// To know which one is more generic we will simply do a Text Contains test, 
            ///     if the LHS contains RHS then LHS is more specific and should be the winner
            ///     if RHS contains LHS then RHS is more specific and should be the winner

            var allWinners = results.Where(r => r.AverageWeight == winnerRule.AverageWeight && r.Passed >= 1)?.ToArray();
            
            if (allWinners.Count() > 1)
            {
                var champion = allWinners[0];

                for (int i = 1; i < allWinners.Count(); i++)
                {
                    var challenger = allWinners[i];
                    champion = ElectWinner(champion, challenger);
                    if (champion == null) champion = allWinners[0];

                }

                if (champion != null)
                    return champion;
            }
            else if (allWinners.Count() == 1) //if there is only 1 winner
            {
                return allWinners[0];
            }

            return winnerRule;
        }

        protected TextClassificationRuleEvaluationResult ElectWinner(TextClassificationRuleEvaluationResult champion, TextClassificationRuleEvaluationResult challenger)
        {
            if (!string.IsNullOrEmpty(champion.Text) && !string.IsNullOrEmpty(challenger.Text))
                if (champion.Text.Contains(challenger.Text))
                    return champion;
            if (!string.IsNullOrEmpty(champion.Text) && !string.IsNullOrEmpty(challenger.Text) && (challenger.Text.Contains(champion.Text)))
                return challenger;
            else
                return null;

            return champion;
        }


        public async Task<TextClassificationRuleEvaluationResult> EvaluateRulesAsync(string text, TextClassificationRules rules)
        {
            TextClassificationRuleEvaluationResult result = new TextClassificationRuleEvaluationResult(rules.Name);

            foreach(TextClassificationRule rule in rules)
            {
                TextClassificationRuleEvaluationResult ruleResult = await Task.FromResult<TextClassificationRuleEvaluationResult>( EvaluateRuleAsync(text, rule));
                result.Passed += ruleResult.Passed;
                result.Failed += ruleResult.Failed;
                result.Weight += ruleResult.Weight;
                if (!string.IsNullOrEmpty(ruleResult.Text))
                    result.Text = ruleResult.Text;
            }
            result.TotalWeight = rules.Sum(r => r.Weight);
            return result;
        }

        public TextClassificationRuleEvaluationResult EvaluateRuleAsync(string text, TextClassificationRule rule)
        {
            TextClassificationRuleEvaluationResult result = new TextClassificationRuleEvaluationResult();
            if (EvaluateRuleItem(text, rule))
            {
                result.Weight = rule.Weight;
                result.Passed++;
                if (rule.RuleType == TextClassificationRuleType.ContainsText)
                    result.Text = rule.Pattern;
            }
            else
            {
                result.Weight = 0;
                result.Failed++;
            }
            return result;
        }

        public bool EvaluateRuleItem(string text, TextClassificationRule rule)
        {
            switch (rule.RuleType)
            {
                case TextClassificationRuleType.Equals:
                    return EqualsText(text, rule.Pattern, rule.Weight);
                case TextClassificationRuleType.ContainsText:
                    return ContainsText(text, rule.Pattern, rule.Weight);
                case TextClassificationRuleType.NotContainText:
                    return NotContainsText(text, rule.Pattern, rule.Weight);
                case TextClassificationRuleType.ContainsRegEx:
                    return ContainsRegEx(text, rule.Pattern, rule.Weight);
                case TextClassificationRuleType.NotContainRegEx:
                    return NotContainsRegEx(text, rule.Pattern, rule.Weight);
                case TextClassificationRuleType.ContainsSimilarText:
                    return HasSimilarText(text, rule.Pattern, rule.Distance, rule.Weight);
                default:
                    return false;
            }
        }


        public bool ContainsText(string text, string contains, float weight = 10)
        {
            if(text.Contains("[") || text.Contains("(") || text.Contains(")") || text.Contains("/") || text.Contains("]"))
                return (text.ToLowerInvariant()
                    .Replace(" ", "")
                    .Contains(contains.ToLowerInvariant().Replace(" ", "")));
            else
                return   
                        (text.ToLowerInvariant()
                        .Replace(" ", "")
                        .Replace("[", "")
                        .Replace("]","")
                        .Replace("(","")
                        .Replace(")","")
                        .Replace("/","")
                        .Contains(contains.ToLowerInvariant().Replace(" ", "")))
                        ;
        }


        //Regex.Replace(s, "[()/]", "")

        public bool NotContainsText(string text, string contains, float weight = 10)
        {
            return !ContainsText(text, contains, weight);
        }

        public bool EqualsText(string text, string contains, float weight = 10)
        {
            return (text.Equals(text, StringComparison.InvariantCultureIgnoreCase));
        }
        public bool NotEqualsText(string text, string contains, float weight = 10)
        {
            return !EqualsText(text, contains, weight);
        }

        public bool ContainsRegEx(string text, string regex, float weight = 10)
        {
            Regex exp = new Regex(regex, RegexOptions.IgnoreCase);
            return (exp.Match(text).Success);
            
        }

        public bool NotContainsRegEx(string text, string regex, float weight = 10)
        {
            return !ContainsRegEx(text, regex, weight);
        }

        public bool HasSimilarText(string text, string otherText , double distance = 0, float weight = 10)
        {
            double editDistance = JaroWinklerDistance.distance(text, otherText);
            return editDistance <= distance;
        }

    }
}
