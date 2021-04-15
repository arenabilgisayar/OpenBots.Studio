
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.Core.Configuration
{
    public class OcrDigitizationService : IDigitizationService
    {
        public OcrResult Execute(IDigitizationSettings configuration, byte[] imageRawBytes)
        {
            OcrResult digitizationResult = null;
            if (configuration is null) return digitizationResult;

            var ocrEngine = OcrFactory.CreateEngine();
            Stream stream = new MemoryStream(imageRawBytes);
            digitizationResult = ocrEngine.PerformRecognition(stream);

            return digitizationResult;
        }
    }
}
