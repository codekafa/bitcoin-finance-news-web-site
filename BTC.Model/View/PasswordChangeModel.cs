using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class PasswordChangeModel
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }

        public int UserID { get; set; }
        public string PasswordAgain { get; set; }

        public string ProcessGuid { get; set; }
    }
}
