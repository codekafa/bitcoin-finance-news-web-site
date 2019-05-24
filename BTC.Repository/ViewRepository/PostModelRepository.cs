using BTC.Core.Base.Repository;
using BTC.Model.Entity;
using BTC.Model.View;
using BTC.Repository.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Repository.ViewRepository
{
    public class PostModelRepository : BaseDapperRepository<BTCConnection, PostModel>
    {
    }
}
