using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.CustomException
{
    public class ValidateException : BaseException
    {
        public ValidateException(string message) : base(message)
        {
            UserMessage = message;
        }

        public ValidateException(string message, Exception innerException) : base(message, innerException)
        {
            OriginalException = innerException;
            UserMessage = message;
        }

        public Exception OriginalException { get; private set; }
        public string UserMessage { get; set; }
    }
}
