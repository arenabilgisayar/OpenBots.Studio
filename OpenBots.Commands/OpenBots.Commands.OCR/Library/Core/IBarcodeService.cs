using System.Collections.Generic;
using System.IO;
using TextXtractor.Ocr.Core;

namespace TextXtractor.Ocr.Core
{
    public interface IBarcodeService
    {
        List<BarcodeBox> PerformRecognition(Stream imageStream);
    }
}