using BTC.Core.Base.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace BTC.Repository.Connection
{
    public class BTCConnection : BaseDatabaseConnection
    {
        public override string ConnectionString { get { return WebConfigurationManager.ConnectionStrings["BTCCon"].ToString(); } }
    }
}
