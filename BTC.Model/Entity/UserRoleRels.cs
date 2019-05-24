using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class UserRoleRels : IEntity
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int RoleID { get; set; }
    }
}
