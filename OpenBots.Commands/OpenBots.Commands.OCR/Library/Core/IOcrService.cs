using System;
using System.IO;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Interface for Implementing an Ocr Service Client
    /// </summary>
    public interface IOcrService : IInitializable, IDisposable
    {
        string ServiceName { get; }


        int SettingsCount();
        /// <summary>
        /// Perform Text Recognition given an Image
        /// </summary>
        /// <param name="imageSteam">Stream for an Image</param>
        /// <returns></returns>
        OcrResult PerformRecognition(Stream imageSteam, int settingIndex = 0);

        ///// <summary>
        ///// Converts the OcrResult into a Full String for use with Unstructured Content
        ///// </summary>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //string ExtractText(OcrResult result);
    }
}
