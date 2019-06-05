using BTC.Base;
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
        [Route("~/görsel/{name}")]
        public ActionResult ContentViever(string name)
        {
            return View();
        }
    }
}