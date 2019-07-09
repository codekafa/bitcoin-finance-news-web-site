using BTC.Core.Base.Repository;
using BTC.Model.View;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository.ViewRepository
{
    public class PageUrlItemRepository : BaseDapperRepository<BTCConnection, PageUrlItem>
    {

        public List<PageUrlItem> GetCategoryList()
        {
            return this.GetByCustomQuery("select ID,Name,Uri from Categories where IsActive  = 1", null);
        }

        public List<PageUrlItem> GetUserListByRoleID(int role_id)
        {
            return this.GetByCustomQuery(@"select u.ID, FirstName + ' ' + LastName  as [Name], u.Uri from Users u 
inner join UserRoleRels ur on ur.UserID = u.ID
where u.IsActive = 1 and u.IsApproved = 1  and ur.RoleID = @RoleID", new { RoleID = role_id });
        }

        public List<PageUrlItem> GetPostList()
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from UserPosts
where IsActive = 1 and IsPublish = 1 ", null);
        }

        public List<PageUrlItem> GetPageList()
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from Pages
where IsActive = 1 and IsPublish = 1 ", null);
        }

        public List<PageUrlItem> GetContentList()
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from ContentViews
where  IsPublish = 1 ", null);
        }


    }
}
