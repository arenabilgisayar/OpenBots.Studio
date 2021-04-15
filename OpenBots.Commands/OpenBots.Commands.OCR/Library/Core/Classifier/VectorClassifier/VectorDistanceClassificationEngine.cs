using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core.Classifier.VectorClassifier
{
    public class VectorDistanceClassificationEngine
    {
        private VectorDistance vectorDistance;
        public VectorDistanceClassificationEngine() {
            vectorDistance = new VectorDistance();
        }

        public IEnumerable<VectorDistancePageClassificationResult> MatchTheForms(List<VectorDistanceForm> form, string formVectorString, double cutoff = 0.95)
        {
            return vectorDistance.MatchTheForms(form, formVectorString, cutoff);
        }
    }
}
