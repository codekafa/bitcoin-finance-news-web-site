using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Model.View
{
    public class AddProductPhotosModel
    {

        public int ProductID { get; set; }

        public List<HttpPostedFileBase> PhotoList { get; set; }

    }
}
