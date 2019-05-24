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

        public bool RegisterApproveMethod { get; set; }
    }
}
