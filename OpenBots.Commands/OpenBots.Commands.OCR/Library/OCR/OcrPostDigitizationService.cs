
using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.Core.Configuration
{
    public class OcrPostDigitizationService : IPostDigitizationService
    {
        public OcrResult Execute(IDigitizationSettings configuration, OcrResult ocrResult)
        {
            return ocrResult;
        }

    }
}
