using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TextXtractor.Ocr.Core;
using TextXtractor.Ocr.Core.Configuration;

namespace TextXtractor.Ocr.Micr
{
    public class MicrDigitizationService : IDigitizationService
    {
        public OcrResult Execute(IDigitizationSettings configuration, byte[] imageRawBytes)
        {
            OcrResult digitizationResult = null;
            if (configuration is null) return digitizationResult;
            digitizationResult = new OcrResult();

            IMicrCodeService micrCodeService = null;
            if (configuration != null)
                micrCodeService = new MicrCodeService(new MicrSettings(configuration));
            else
                micrCodeService = new MicrCodeService(new MicrSettings());

            IOcrEngine engine = EngineFactory.CreateMicrEngine(micrCodeService);
            var micrCode = engine.PerformRecognition(imageRawBytes)?.MicrCodes?.FirstOrDefault();
            if (micrCode != null)
                digitizationResult.MicrCodes.Add(micrCode);

            return digitizationResult;
        }
    }

}
