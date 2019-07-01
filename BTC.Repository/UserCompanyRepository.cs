using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository
{
    public class UserCompanyRepository : BaseDapperRepository<BTCConnection,UserCompanies>
    {

        public UserCompanies GetCompanyByUserId(int user_id)
        {
            return GetByCustomQuery("select * from UserCompanies where UserID = @UserID", new { UserID = user_id }).FirstOrDefault();
        }

    }
}
