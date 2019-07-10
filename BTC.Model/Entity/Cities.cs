using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class Cities : IEntity
    {

        public int ID { get; set; }

        public int CountryID { get; set; }

        public string Name { get; set; }

        public string Uri { get; set; }

        public string Code { get; set; }

    }
}
