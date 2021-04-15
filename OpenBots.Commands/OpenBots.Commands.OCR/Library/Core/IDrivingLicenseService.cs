using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public interface IDrivingLicenseService : IInitializable, IDisposable
    {
        DrivingLicenseInfo PerformRecognition(byte[] imageRawBytes);
        DrivingLicenseInfo PerformRecognition(Stream imageSteam);
    }
}
