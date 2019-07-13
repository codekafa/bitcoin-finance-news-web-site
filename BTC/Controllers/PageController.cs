using BTC.Base;
using BTC.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class PageController : BaseController
    {

        PagesManager _pageM;
        public PageController()
        {
            _pageM = new PagesManager();
        }

        [Route("~/sayfalar/{parent_name}/{page_name}")]
        public ActionResult GetPage(string page_name)
        {
            var page = _pageM.GetPageModelByUri(page_name);


            if (page == null || !page.IsActive || !page.IsPublish)
            {
                return RedirectToErrorPage(new Model.Response.ResponseModel { IsSuccess = false, Message = "Aradığınız sayfa bulunamadı!" });
            }

            return View(page);
        }
    }
}