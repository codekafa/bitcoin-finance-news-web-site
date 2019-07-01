using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Model.View
{
    public class UpdateUserModel
    {

        public int ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public HttpPostedFileBase ProfilePhotoUrl { get; set; }

        public bool IsChangeProfilePhoto { get; set; }
        public string PhotoName { get; set; }

        public string PhotoSaveBaseUrl { get; set; }
        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Linkedin { get; set; }

        public string Summary { get; set; }

        public int CompanyID { get; set; }

        public int CompanyCity { get; set; }
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyDescription { get; set; }

        public string CompanyPhone { get; set; }

        public string CompanyEmail { get; set; }
        public UserCompanies Company { get; set; }

    }
}
