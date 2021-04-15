using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using TextXtractor.Ocr.Barcode;
using TextXtractor.Ocr.Core;
using TextXtractor.Ocr.DrivingLicense;
using TextXtractor.Ocr.GoogleCloud;
using TextXtractor.Ocr.Micr;

namespace TextXtractor.Ocr
{
    public static class OcrFactory
    {
        public static ISettings CreateSettings()
        {
            var primary = Settings.CreateEnvironmentConfiguration();
            var fallBack = Settings.CreateJsonConfiguration();
            return new Settings(primary, fallBack);
        }

        public static IOcrEngine CreateEngine(IOcrService service = null)
        {
            ISettings settings = CreateSettings();

            OcrEngineSetting engineSetting = OcrEngineSetting.Create(settings);
            if (string.IsNullOrEmpty(engineSetting.Service))
                engineSetting.Service = "Google";

            if (service == null)
            {
                if (engineSetting.Service.ToUpperInvariant().StartsWith("G"))
                    service = GoogleVisionFactory.Create(settings);
                if (engineSetting.Service.ToUpperInvariant().StartsWith("M"))
                    service = MicrosoftAzure.MicrosoftVisionFactory.Create(settings);
            }

            if (service == null)
                throw new ArgumentNullException("Cannot Create Service. Configuration may be incorrect");

            //IBarcodeService barcodeService = BarcodeServiceFactory.Create(settings);
            //IOcrEngine engine = EngineFactory.Create(engineSetting, service, barcodeService);

            IMicrCodeService micrCodeService = new MicrCodeService(new MicrSettings());
            IDrivingLicenseService drivingLicenseService = new DrivingLicenseService(new DrivingLicenseSetting());

            IOcrEngine engine = EngineFactory.Create(engineSetting, service, micrCodeService, drivingLicenseService);

            return engine;

        }

    }
}
