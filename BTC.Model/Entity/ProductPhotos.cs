using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class ProductPhotos : IEntity
    {

        public int ID { get; set; }

        public int ProductID { get; set; }

        public string Photo { get; set; }

        public bool IsMain { get; set; }
    }
}
