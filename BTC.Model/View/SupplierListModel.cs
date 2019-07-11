using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class SupplierListModel : IEntity
    {

        public int UserID { get; set; }


        public int CompanyID { get; set; }

        public  string Name { get; set; }

        public string Uri { get; set; }
        public int ProductCount { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }


        public string Phone { get; set; }

        public string  Address { get; set; }

        public string Description { get; set; }
    }
}
