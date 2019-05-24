using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class Comments : IEntity
    {
        public int ID { get; set; }

        public int TypeID { get; set; }

        public int? UserID { get; set; }

        public int ParentID { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsPublish { get; set; }
    }
}
