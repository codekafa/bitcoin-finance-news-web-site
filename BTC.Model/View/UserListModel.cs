using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class UserListModel : IEntity
    {

        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVip { get; set; }
        public bool IsMember { get; set; }
        public bool IsWriter { get; set; }
        public bool IsSupplier { get; set; }

        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
