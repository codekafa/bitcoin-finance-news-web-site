using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Model.View
{
    public class AddSliderModel
    {
        public int ID { get; set; }

        public string PhotoUrl { get; set; }

        public string SmallTitle { get; set; }

        public string MediumTitle { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }


        public string SaveBaseAddress { get; set; }
        public HttpPostedFileBase SliderImage { get; set; }
    }
}
