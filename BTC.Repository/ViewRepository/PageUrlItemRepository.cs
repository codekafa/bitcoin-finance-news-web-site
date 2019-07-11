using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.Response;
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

        public List<PageUrlItem> GetCategoryList(int? category_id = null)
        {
            return this.GetByCustomQuery("select ID,Name,Uri from Categories where IsActive  = 1 and (@CategoryID is null or ID = @CategoryID)", new { CategoryID = category_id });
        }

        public List<PageUrlItem> GetUserListByRoleID(int role_id, int? user_id = null)
        {
            return this.GetByCustomQuery(@"select u.ID, FirstName + ' ' + LastName  as [Name], u.Uri from Users u 
inner join UserRoleRels ur on ur.UserID = u.ID
where u.IsActive = 1 and u.IsApproved = 1  and ur.RoleID = @RoleID and (@UserID is null or u.ID = @UserID)", new { RoleID = role_id , UserID = user_id });
        }

        public List<PageUrlItem> GetPostList(int? post_id=null)
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from UserPosts
where IsActive = 1 and IsPublish = 1 and (@ID is null or ID = @ID)", new { ID = post_id });
        }

        public List<PageUrlItem> GetPageList(int? page_id=null)
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from Pages
where IsActive = 1 and IsPublish = 1 and (@PageID is null or ID = @PageID) ", new { PageID = page_id });
        }

        public List<PageUrlItem> GetContentList(int? content_id = null)
        {
            return this.GetByCustomQuery(@"select ID, Title as [Name], Uri from ContentViews
where  IsPublish = 1  and (@ID is null  or ID =@ID)", new { ID = content_id });
        }

        public List<PageUrlItem> GetCityPageList(bool is_regional)
        {
            return this.GetByCustomQuery(@"select c.ID,c.Name,c.Uri from Cities c
inner join Countries co on co.ID = c.CountryID
inner join UserCompanies com on com.CityID = c.ID
where co.IsRegional = @IsRegional and com.IsActive = 1", new { IsRegional = is_regional });
        }

    }
}
