
using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.Core.Configuration
{
    public interface IDigitizationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="imageRawBytes"></param>
        /// <returns></returns>
        OcrResult Execute(IDigitizationSettings configuration, byte[] imageRawBytes);
    }
}
