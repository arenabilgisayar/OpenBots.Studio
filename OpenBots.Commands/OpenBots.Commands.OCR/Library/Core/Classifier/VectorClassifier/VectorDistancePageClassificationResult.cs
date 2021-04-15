using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core.Classifier.VectorClassifier
{
    public class VectorDistancePageClassificationResult
    {
        public string FormID { get; set; }
        public string FormName { get; set; }
        public double SimilarityValue { get; set; }

    }

}
