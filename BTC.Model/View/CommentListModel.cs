using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public  class CommentListModel : IEntity
    {
        public int ID { get; set; }
        public string PostTitle { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public string CreateDateAsString { get { return CreateDate.ToString("dd/MM/yyyy HH:mm"); } }
    }
}
