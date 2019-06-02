using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class UserPosts : IEntity
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int CategoryID { get; set; }

        public string TopPhotoUrl { get; set; }
        public string    Title { get; set; }

        public string Body { get; set; }

        public string Tags { get; set; }

        public string Uri { get; set; }

        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string Summary { get; set; }

        public int ViewCount { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }

        public bool IsActive { get; set; }
    }
}
