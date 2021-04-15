using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public interface IMicrCodeService : IInitializable, IDisposable
    {
        MicrCode PerformRecognition(byte[] imageRawBytes);
        MicrCode PerformRecognition(Stream imageSteam);
    }
}
