using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;
using TextXtractor.Ocr.Core.Configuration;

namespace TextXtractor.Ocr.Micr
{
    public class MicrPostDigitizationService : IPostDigitizationService
    {
        public OcrResult Execute(IDigitizationSettings configuration, OcrResult ocrResult)
        {
            return ocrResult;
        }

    }
}
