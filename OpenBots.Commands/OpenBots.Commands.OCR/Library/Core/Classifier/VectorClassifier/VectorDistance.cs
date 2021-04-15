using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TextXtractor.Ocr.Core.Classifier.VectorClassifier;

namespace TextXtractor.Ocr.Core
{
    public class VectorDistance
    {
        public IEnumerable<VectorDistancePageClassificationResult> MatchTheForms(List<VectorDistanceForm> vectorDistanceForm, string incomingVectorString, double cutoff = 0.98)
        {
            Vector<double> incomingVector = StringToVector(incomingVectorString);
            Dictionary<VectorDistanceForm, Vector<double>> formVectors = new Dictionary<VectorDistanceForm, Vector<double>>();
            Dictionary<VectorDistanceForm, double> vectorSimilarity = new Dictionary<VectorDistanceForm, double>();
            double cutOffSimilarity = 1 - cutoff;

            // Convert incoming Vectors as Serialized String to Vector object
            foreach (var form in vectorDistanceForm)
            {
                formVectors.Add(form, StringToVector(form.HuMoments));
            }
            foreach (var vector in formVectors)
            {
                double similarity = MomentSimilarity(vector.Value, incomingVector);
                if (similarity < cutOffSimilarity)
                    vectorSimilarity.Add(vector.Key, similarity);
            }
            var winner = vectorSimilarity.Where(kp => kp.Value <= cutoff).OrderBy(kp => kp.Value).FirstOrDefault();
            double winningConfidence = winner.Value;
            return GetMatchedFormsResult(vectorSimilarity.Where(kp => kp.Value <= cutoff && kp.Value == winningConfidence));
        }

        //def moment_similarity(a, b):    return np.linalg.norm(a-b)
        public double MomentSimilarity(Vector<double> lhs, Vector<double> rhs)
        {
            var result = lhs - rhs;
            return result.L2Norm();
        }

        public double MomentSimilarity(string lhs, string rhs)
        {
            return MomentSimilarity(StringToVector(lhs), StringToVector(rhs));
        }

        public Vector<double> StringToVector(string vectorString)
        {
            if (string.IsNullOrEmpty(vectorString))
                vectorString = "0,0,0,0,0,0,0".Replace("0", double.MaxValue.ToString());

            List<string> numberstrings = vectorString.Replace("\n", "").Replace("\t", "").Replace(" ", "").Replace(",", " ").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            List<double> numbers = new List<double>();
            foreach (string number in numberstrings)
            {
                try
                {
                    if (!string.IsNullOrEmpty(number))
                    {

                        double dblNum = 0;
                        if (double.TryParse(number, out dblNum))
                            numbers.Add(dblNum);
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException(string.Format("Message:{0} \n Vector:{1} \n Number:{2}", ex.Message, vectorString, number), ex);
                }

            }
            //double[] numbers = vectorString.Replace(",", " ").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s)).ToArray();

            return CreateVector.DenseOfArray<double>(numbers.ToArray());
        }



        public IEnumerable<VectorDistancePageClassificationResult> GetMatchedFormsResult(IEnumerable<KeyValuePair<VectorDistanceForm, double>> formVectors)
        {
            IList<VectorDistancePageClassificationResult> results = new List<VectorDistancePageClassificationResult>(); 
            foreach (var vector in formVectors)
            {
                VectorDistancePageClassificationResult vectorDistancePageClassificationResult = new VectorDistancePageClassificationResult()
                {
                    FormID = vector.Key.FormId,
                    FormName = vector.Key.FormName,
                    SimilarityValue = vector.Value
                };
                results.Add(vectorDistancePageClassificationResult);
            }
            return results;
        }
    }
}
