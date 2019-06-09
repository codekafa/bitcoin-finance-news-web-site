using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Model.View
{
    public class AddProductModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Uri { get; set; }
        public decimal? Price { get; set; }

        public string Tags { get; set; }
        public bool IsPublish { get; set; }

        public string BasePhotoPath { get; set; }
        public HttpPostedFileBase MainImage { get; set; }
        public List<HttpPostedFileBase> Photos { get; set; }


    }
}
