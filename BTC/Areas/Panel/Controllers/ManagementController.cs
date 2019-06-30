using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Areas.Panel.Controllers
{
    public class ManagementController : Controller
    {

        [Route("panel")]
        [Route("panel/dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}