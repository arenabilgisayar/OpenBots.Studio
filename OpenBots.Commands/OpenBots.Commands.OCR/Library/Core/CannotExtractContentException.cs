using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Exception when Image cannot be Extracted by the Ocr Engine
    /// </summary>
    public class CannotExtractContentException : IOException
    {
        public CannotExtractContentException()
        {
        }

        public CannotExtractContentException(string message) : base(message)
        {
        }

        public CannotExtractContentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CannotExtractContentException(string message, int hresult) : base(message, hresult)
        {
        }

        protected CannotExtractContentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
