using System;

namespace SatisfactorySaveParser.ZLib
{
    public class ZLibException : Exception
    {
        public ZLibReturnCode ReturnCode { get; set; }

        public ZLibException()
        {
        }

        public ZLibException(string message) : base(message)
        {
        }

        public ZLibException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ZLibException(ZLibReturnCode errorCode, string lastStreamError)
            : base(GetMsg(errorCode, lastStreamError))
        {
            ReturnCode = errorCode;
        }

        private static string GetMsg(ZLibReturnCode errorCode, string lastStreamError)
        {
            var msg = $"ZLib error {errorCode}";

            if (!String.IsNullOrEmpty(lastStreamError))
                msg += $" ({lastStreamError})";

            return msg;
        }
    }
}
