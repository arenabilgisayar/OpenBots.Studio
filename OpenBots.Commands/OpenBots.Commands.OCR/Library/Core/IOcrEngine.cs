using System;
using System.IO;

namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Interface for Ocr Engine
    /// </summary>
    public interface IOcrEngine: IInitializable, IDisposable
    {
        OcrResult PerformRecognition(byte[] imageData);

        OcrResult PerformRecognition(string imagePath);

        OcrResult PerformRecognition(Stream imageSteam);
    }

}