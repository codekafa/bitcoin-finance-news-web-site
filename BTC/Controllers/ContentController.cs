using BTC.Base;
using BTC.Business.Managers;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class ContentController : BaseController
    {
        ContentViewManager _contentM;
        public ContentController()
        {
            _contentM = new ContentViewManager();
        }

        [Route("~/gorsel/{name}")]
        public ActionResult ContentViever(string name)
        {
            var content = _contentM.GetContentViewByUri(name);
            return View(content);
        }
    }
}