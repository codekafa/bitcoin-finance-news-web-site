using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.Connection
{
    public abstract class BaseDatabaseConnection : IConnectionFactory
    {

        public BaseDatabaseConnection()
        {
        }

        private IDbConnection _conn;
        public IDbConnection GetConnection
        {
            get
            {
                var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                var conn = factory.CreateConnection();
                conn.ConnectionString = ConnectionString;
                conn.Open();
                _conn = conn;
                if (_conn.State != ConnectionState.Open)
                    _conn.Open();

                return _conn;
            }
        }


        public virtual string ConnectionString { get; }

        public void Dispose()
        {

        }
    }
}
