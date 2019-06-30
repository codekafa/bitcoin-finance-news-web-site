using BTC.Core.Base.Model;
using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public  class DasboardMainModel : IEntity
    {

        public int CategoryCount { get; set; }

        public int PostCount { get; set; }
        public int ProductCount { get; set; }
        public int UserCount { get; set; }
        public int ContentCount { get; set; }

        public int VipCount { get; set; }

        public int CommentCount { get; set; }

        public int NewsCount { get; set; }

        public List<Comments> Comments { get; set; }
        public List<UserPosts> Posts { get; set; }
        public List<Users> Users { get; set; }
        public List<UserPosts> News { get; set; }
        public List<UserPosts> BestPosts { get; set; }

    }
}
