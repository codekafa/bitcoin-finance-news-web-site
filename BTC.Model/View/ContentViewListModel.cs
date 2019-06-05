using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class ContentViewListModel : IEntity
    {
        public string Title { get; set; }
        public int RowNumber { get; set; }
        public string Uri { get; set; }

    }
}
