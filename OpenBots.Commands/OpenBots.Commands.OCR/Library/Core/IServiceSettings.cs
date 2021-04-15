using System;
using System.Collections.Generic;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public interface IDigitizationSettings
    {
        string Name { get; set; }
        bool Enabled { get; set; }
        string Url { get; set; }
        string ApiKey { get; set; }
        string ApiKeyPath { get; set; }

        IDigitizationSettings GetDigitizationSettings();
    }
}
