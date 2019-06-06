using BTC.Model.Entity;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class CurrentUserModel
    {

        private bool? _isWriter { get; set; }
        private bool? _isAdmin { get; set; }
        private bool? _isMember { get; set; }
        public  List<ContentViewListModel> ContentViews { get; set; }
        private bool? _isSupplier { get; set; }
        public CurrentUserModel()
        {
            Roles = new List<UserRoleRels>();
            ContentViews = new List<ContentViewListModel>();
        }
        public Users CurrentUser { get; set; }

        public List<UserRoleRels> Roles { get; set; }
        public bool IsWriter
        {
            get
            {
                if (!_isWriter.HasValue)
                {
                    _isWriter = Roles.Where(x => x.RoleID == (int)EnumVariables.Roles.Writer).Count() > 0;
                }
                return _isWriter.Value;
            }
        }
        public bool IsAdmin
        {
            get
            {
                if (!_isAdmin.HasValue)
                {
                    _isAdmin = Roles.Where(x => x.RoleID == (int)EnumVariables.Roles.Admin).Count() > 0;
                }
                return _isAdmin.Value;
            }
        }
        public bool IsMember
        {
            get
            {
                if (!_isMember.HasValue)
                {
                    _isMember = Roles.Where(x => x.RoleID == (int)EnumVariables.Roles.Member).Count() > 0;
                }
                return _isMember.Value;
            }
        }
        public bool IsSupplier
        {
            get
            {
                if (!_isSupplier.HasValue)
                {
                    _isSupplier = Roles.Where(x => x.RoleID == (int)EnumVariables.Roles.Supplier).Count() > 0;
                }
                return _isSupplier.Value;
            }
        }
        public string FullName
        {
            get
            {
                return CurrentUser.FirstName + " " + CurrentUser.LastName;
            }
        }
    }
}
