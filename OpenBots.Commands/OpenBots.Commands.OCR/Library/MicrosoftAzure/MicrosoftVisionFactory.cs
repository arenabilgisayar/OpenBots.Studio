using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.MicrosoftAzure
{
    public static class MicrosoftVisionFactory
    {
        public static IOcrService Create(ISettings setting)
        {
            return Create(MicrosoftVisionOcrSetting.Create(setting));
        }

        public static IOcrService Create(ServiceSettings<MicrosoftVisionOcrSetting> settings)
        {
            IOcrService ocrService = new MicrosoftVisionOcrService(settings);
            ocrService.Init();
            return ocrService;
        }

    }
}
