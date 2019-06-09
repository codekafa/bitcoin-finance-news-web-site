using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class UserProducts : IEntity
    {

        public int ID { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsPublish { get; set; }
    }
}
