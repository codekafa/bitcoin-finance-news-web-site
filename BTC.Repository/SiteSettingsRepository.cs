using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.View;
using BTC.Repository.Connection;
using System.Linq;

namespace BTC.Repository
{
    public class SiteSettingsRepository : BaseDapperRepository<BTCConnection, SiteSettings>
    {

        public DasboardMainModel GetDashboard()
        {
            var viewRepo = new BaseDapperRepository<BTCConnection, DasboardMainModel>();
            var result = viewRepo.GetByCustomQuery(@"
                        select 
                        COUNT(*) as [CategoryCount],
                        (select COUNT(*) from UserPosts where CategoryID != 8) as PostCount,
                        (select COUNT(*) from UserPosts where CategoryID = 8) as NewsCount,
                        (select COUNT(*)from UserProducts) as ProductCount,
                        (select COUNT(*) from Users) as UserCount,
                        (select COUNT(*) from ContentViews) as ContentCount,
                        (select COUNT(*) from Users where IsVip = 1 ) as VipCount,
                        (select COUNT(*) from Users where IsVip = 1 ) as CommentCount
                        from Categories c
                                        ", null).FirstOrDefault();
            result.BestPosts = new UserPostRepository().GetByCustomQuery("select top 10 * from UserPosts where IsPublish = 1 order by ViewCount desc", null);
            result.Posts = new UserPostRepository().GetByCustomQuery("select top 10 * from UserPosts where IsPublish = 1 and CategoryId != 8 order by CreateDate desc", null);
            result.Comments = new CommentRepository().GetByCustomQuery("select top 10 * from Comments  order by CreateDate desc", null);

            result.News = new UserPostRepository().GetByCustomQuery("select top 10 * from UserPosts where IsPublish = 1 and CategoryId = 8 order by CreateDate desc", null);
            result.Users = new UserRepository().GetByCustomQuery("select top 10 * from Users order by CreateDate desc", null);
            return result;

        }

    }
}
