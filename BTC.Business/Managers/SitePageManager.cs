using BTC.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Business.Managers
{
    public class SitePageManager
    {
        SitePageRepository _siteRepo;
        public SitePageManager()
        {
            _siteRepo = new SitePageRepository();
        }

    }
}
