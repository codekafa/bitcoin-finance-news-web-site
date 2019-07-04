using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class MainSliderSettings : IEntity
    {

        public int ID { get; set; }

        public string PhotoUrl { get; set; }

        public string SmallTitle { get; set; }

        public string MediumTitle { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }
    }
}
