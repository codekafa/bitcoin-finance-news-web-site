using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.View;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository
{
    public class UserRepository : BaseDapperRepository<BTCConnection,Users>
    {

        public Users CreateUser(Users userModel)
        {
            userModel.ID = this.Insert(userModel);
            return userModel;
        }

        public Users GetByUserID(int user_id)
        {
            var  user = this.GetByCustomQuery("select * from Users where ID = @ID", new { ID = user_id }).FirstOrDefault();
            return user;
        }

        public Users GetByUserEmailAndPassword(string email, string password)
        {
            var user = this.GetByCustomQuery("select * from Users where Email = @Email and Password = @Password", new { Email = email, Password = password }).FirstOrDefault();
            return user;
        }
        public Users GetByUserEmail(string email)
        {
            var user = this.GetByCustomQuery("select * from Users where Email = @Email", new { Email = email}).FirstOrDefault();
            return user;
        }


    }
}
