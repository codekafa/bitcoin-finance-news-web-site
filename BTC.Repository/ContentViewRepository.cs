using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.View;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository
{
    public class ContentViewRepository : BaseDapperRepository<BTCConnection, ContentViews>
    {
        public List<ContentViewListModel> GetPublishedViewList()
        {
            var cRepo = new BaseDapperRepository<BTCConnection, ContentViewListModel>();
            var list = cRepo.GetByCustomQuery("select Title, Uri, RowNumber, CanSeeUser,CanSeeTrader,CanSeeWriter,CanSeeVip,CanSeeMember from ContentViews where IsPublish = 1 ", null);
            return list;
        }
    }
}
