using BTC.Core.Base.Model;
using System.ComponentModel.DataAnnotations;

namespace BTC.Model.Entity
{
    public class SiteSettings : IEntity
    {

        public int ID { get; set; }

        [Required]
        public string SiteTitle { get; set; }
        [Required]
        public string SiteDescription { get; set; }
        [Required]
        public string SiteKeywords { get; set; }

        [Required]
        public string ComtactEmail { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Linkedin { get; set; }

        public bool RegisterApproveMethod { get; set; }
    }
}
