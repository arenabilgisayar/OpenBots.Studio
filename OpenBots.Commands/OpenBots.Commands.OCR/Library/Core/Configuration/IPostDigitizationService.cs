
using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.Core.Configuration
{
    public interface IPostDigitizationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="ocrResult"></param>
        /// <returns></returns>
        OcrResult Execute(IDigitizationSettings configuration, OcrResult ocrResult);
    }
}
