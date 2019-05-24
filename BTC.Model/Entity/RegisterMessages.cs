using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class RegisterMessages : IEntity
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string MessageBody { get; set; }

        public string MessageCode { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }
    }
}
