using System;
using System.Runtime.Serialization;

namespace OpenBots.Commands.Documents.Exceptions
{
    [Serializable]
    internal class CannotSubmitDocumentToServiceException : Exception
    {
        public CannotSubmitDocumentToServiceException()
        {
        }

        public CannotSubmitDocumentToServiceException(string message) : base(message)
        {
        }

        public CannotSubmitDocumentToServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotSubmitDocumentToServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}