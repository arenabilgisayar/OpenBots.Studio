using System;
using System.Collections.Generic;
using System.Text;
using TextXtractor.Ocr.Core;
using TextXtractor.Ocr.Core.Configuration;

namespace TextXtractor.Ocr.DrivingLicense
{
    public class DrivingLicensePreDigitizationService : IPreDigitizationService
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
