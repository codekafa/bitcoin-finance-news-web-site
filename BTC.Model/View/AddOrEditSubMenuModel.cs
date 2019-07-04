using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Model.View
{
    public class AddOrEditSubMenuModel
    {

        public int ID { get; set; }

        public int ParentID { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string MenuIconUrl { get; set; }

        public bool IsActive { get; set; }

        public bool IsStatic { get; set; }

        public bool IsChangeMenuIcon { get; set; }
        public HttpPostedFileBase MenuIcon { get; set; }

    }
}
