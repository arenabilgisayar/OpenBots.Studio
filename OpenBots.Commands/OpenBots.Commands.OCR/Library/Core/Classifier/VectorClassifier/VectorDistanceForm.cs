using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core.Classifier.VectorClassifier
{
    public class VectorDistanceForm
    {
        public string FormId { get; set; }

        public string FormName { get; set; }

        public string FormEdition { get; set; }

        public int? PageNumber { get; set; }

        public float? Confidence { get; set; }
        public string HuMoments { get; set; }
    }
}
