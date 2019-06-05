using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public  class ContentViews : IEntity
    {
        public int ID { get; set; }

        public int RowNumber { get; set; }

        public string Title { get; set; }

        public string Uri { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string ContentBody { get; set; }

        public bool IsPublish { get; set; }
        public bool CanSeeUser { get; set; }

        public bool CanSeeMember { get; set; }

        public bool CanSeeWriter { get; set; }

        public bool CanSeeTrader { get; set; }

        public bool CanSeeVip { get; set; }
    }
}
