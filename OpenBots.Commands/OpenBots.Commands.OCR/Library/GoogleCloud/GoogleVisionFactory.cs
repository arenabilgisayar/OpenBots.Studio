using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.GoogleCloud
{
    public static class GoogleVisionFactory 
    {
        public static IOcrService Create(ISettings setting)
        {
            return Create(GoogleVisionOcrSetting.Create(setting));
        }

        public static IOcrService Create(ServiceSettings<GoogleVisionOcrSetting> settings)
        {
            IOcrService ocrService = new GoogleVisionOcrService(settings);
            ocrService.Init();
            return ocrService;
        }

    }
}
