using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.Attributes
{
    public class DapperAttribute : Attribute
    {
        private bool _identity = false;
        private bool _isEntityColum = true;

        public bool IsEntityColumn
        {
            get { return _isEntityColum; }
            set { _isEntityColum = value; }
        }

        public bool Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }
    }
}
