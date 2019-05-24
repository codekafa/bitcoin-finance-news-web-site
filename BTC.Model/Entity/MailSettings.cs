using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class MailSettings : IEntity
    {
        public int ID { get; set; }


        public string SmtpAddress { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool ApproveSendMailNewUser { get; set; }
    }
}
