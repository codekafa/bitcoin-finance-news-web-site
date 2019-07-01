using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class UserCompanies : IEntity
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int CityID { get; set; }

        public bool IsActive { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
