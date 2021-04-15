using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TextXtractor.Ocr.Core;
using TextXtractor.Ocr.Core.Configuration;

namespace TextXtractor.Ocr.DrivingLicense
{
    public class DrivingLicenseDigitizationService : IDigitizationService
    {
        public OcrResult Execute(IDigitizationSettings configuration, byte[] imageRawBytes)
        {
            OcrResult digitizationResult = null;
            if (configuration is null) return digitizationResult;
            digitizationResult = new OcrResult();

            IDrivingLicenseService drivingLicenseService = null;
            if (configuration != null)
                drivingLicenseService = new DrivingLicenseService(new DrivingLicenseSetting(configuration));
            else
                drivingLicenseService = new DrivingLicenseService(new DrivingLicenseSetting());

            IOcrEngine engine = EngineFactory.CreateDrivingLicenseEngine(drivingLicenseService);
            var drivingLicenseInfo = engine.PerformRecognition(imageRawBytes)?.DrivingLicenses?.FirstOrDefault();
            if (drivingLicenseInfo != null)
                digitizationResult.DrivingLicenses.Add(drivingLicenseInfo);

            return digitizationResult;
        }
    }

}
