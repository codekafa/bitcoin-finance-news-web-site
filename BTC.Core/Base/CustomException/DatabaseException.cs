using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.CustomException
{

    public class DatabaseException : BaseException
    {

        public Exception OriginalException { get; private set; }

        public DatabaseException(string message) : base(message)
        {

        }

        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
            OriginalException = innerException;
        }
    }
}
