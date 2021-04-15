using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Factory for Ocr
    /// </summary>
    public static class EngineFactory
    {
        /// <summary>
        /// Creates an Instance of the ocr Engine, Given an Ocr Service
        /// </summary>
        /// <param name="ocrService">Instance of an Ocr Service</param>
        /// <returns>Instance of OcrEngine</returns>
        public static IOcrEngine Create(OcrEngineSetting engineSetting, IOcrService ocrService/*, IBarcodeService barcodeService*/, IMicrCodeService micrCodeService, IDrivingLicenseService drivingLicenseService)
        {
            var engine = new OcrEngine(engineSetting, ocrService/*, barcodeService*/, micrCodeService, drivingLicenseService);
            engine.Init();
            return engine;
        }

        public static IOcrEngine CreateMicrEngine(IMicrCodeService micrCodeService)
        {
            var engine = new OcrEngine();
            engine.InitMicrService(micrCodeService);
            return engine;
        }

        public static IOcrEngine CreateDrivingLicenseEngine(IDrivingLicenseService drivingLicenseService)
        {
            var engine = new OcrEngine();
            engine.InitDrivingLicenseService(drivingLicenseService);
            return engine;
        }

    }
}
