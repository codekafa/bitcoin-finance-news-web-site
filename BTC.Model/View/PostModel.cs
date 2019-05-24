using BTC.Core.Base.Model;
using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class PostModel : IEntity
    {
        private string Writer { get; set; }
        private string Category { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public string TopPhotoUrl { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Tags { get; set; }

        [Required]
        public string Uri { get; set; }

        [Required]
        public string MetaTitle { get; set; }

        [Required]
        public string MetaKeywords { get; set; }

        [Required]
        public string Summary { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        public bool IsActive { get; set; }

    }
}
