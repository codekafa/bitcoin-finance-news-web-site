using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.Entity
{
    public class Users : IEntity
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Linkedin { get; set; }

        public string Summary { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsVip { get; set; }

        public bool IsApproved { get; set; }

        public bool IsActive { get; set; }

    }
}
