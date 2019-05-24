using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class LoginAttempts : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserIpAddress { get; set; }
        public string IP4Address { get; set; }
        public string MacAddress { get; set; }
        public string GeoLocation { get; set; }
        public string HostName { get; set; }
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string OpSystem { get; set; }
        public bool BrowserJavascript { get; set; }
        public bool Result { get; set; }
        public string Response { get; set; }
        public DateTime CreateDate { get; set; }
        public string CalledURL { get; set; }
    }
}
