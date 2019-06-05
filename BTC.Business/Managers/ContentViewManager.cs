using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.View;
using BTC.Repository;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class ContentViewManager
    {

        ContentViewRepository _contentRepo;
        public ContentViewManager()
        {
            _contentRepo = new ContentViewRepository();
        }
        public List<ContentViews> GetPublishedContents()
        {
            var list = _contentRepo.GetByCustomQuery("select * from ContentViews where IsPublish = 1", null);
            return list;
        }
        public List<ContentViewListModel> GetListViewPublished()
        {
            var cRepo = new BaseDapperRepository<BTCConnection, ContentViewListModel>();
            var list = cRepo.GetByCustomQuery("select Title, Uri, RowNumber from ContentViews where IsPublish = 1 ", null);
            return list;

        }
        public ContentViews GetContentViewByUri(string uri)
        {
            var content = _contentRepo.GetByCustomQuery("select * from ContentViews where IsPublish = 1 and Uri = @Uri", new { Uri = uri }).FirstOrDefault();
            return content;
        }
        public ContentViews GetContentViewByID(int id)
        {
            var content = _contentRepo.GetByCustomQuery("select * from ContentViews where  ID = @ID", new { ID = id }).FirstOrDefault();
            return content;
        }

    }
}
