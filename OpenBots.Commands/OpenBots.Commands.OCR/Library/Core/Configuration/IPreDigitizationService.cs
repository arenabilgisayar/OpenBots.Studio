using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPreDigitizationService 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="imageRawBytes"></param>
        /// <returns></returns>
        byte[] Execute(IDigitizationSettings configuration, byte[] imageRawBytes);
    }
}
