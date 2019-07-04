using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class MainPageSettings : IEntity
    {

        public int ID { get; set; }

        public string OneStepTitle { get; set; }

        public string OneStepDescription { get; set; }

        public string OneStepButtonText { get; set; }

        public string OneStepButtonUrl { get; set; }

        public string TwoStepTitle { get; set; }

        public string TwoStepDescription { get; set; }

        public string TwoStepButtonText { get; set; }

        public string TwoStepButtonUrl { get; set; }

        public string ThreeStepTitle { get; set; }

        public string ThreeStepDescription { get; set; }

        public string ServiceOneTitle { get; set; }

        public string ServiceOneDescription { get; set; }

        public string ServiceTwoTitle { get; set; }

        public string ServiceTwoDescription { get; set; }

        public string ServiceThreeTitle { get; set; }

        public string ServiceThreeDescription { get; set; }
    }
}
