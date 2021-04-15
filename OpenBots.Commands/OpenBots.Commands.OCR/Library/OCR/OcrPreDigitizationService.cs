
using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core.Configuration
{
    public class OcrPreDigitizationService : IPreDigitizationService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="imageRawBytes"></param>
        /// <returns></returns>
        public byte[] Execute(IDigitizationSettings configuration, byte[] imageRawBytes)
        {
            return imageRawBytes;
        }
    }
}
