using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.CustomException
{
    public class BusinessException : BaseException
    {
        public Exception OriginalException { get; private set; }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            OriginalException = innerException;
        }
    }
}
