using BTC.Core.Base.Model;
using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BTC.Model.View
{
    public class PostModel : IEntity
    {
        public string Writer { get; set; }
        public string Category { get; set; }

        public string WriterPhoto { get; set; }
        public int ID { get; set; }
        public HttpPostedFileBase MainImage { get; set; }

        public int UserID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public string TopPhotoUrl { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Body { get; set; }
        [Required]
        public string Tags { get; set; }

        [Required]
        public string Uri { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        [Required]
        public string Summary { get; set; }

        public bool IsChangeMainImage { get; set; }

        public int ViewCount { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsActive { get; set; }

        public string FileSaveMap { get; set; }

        public List<Comments> PostComments { get; set; }
    }
}
