using BTC.Business.Managers;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    public class PageController : Controller
    {

        PagesManager _pageM;
        public PageController()
        {
            _pageM = new PagesManager();
        }

        [Route("~/sayfalar")]
        public ActionResult Pages()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getPages()
        {
            var list = _pageM.GetAllPages();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [Route("yeni-sayfa-ekle")]
        public ActionResult AddNewPage()
        {
            return View();
        }


        [Route("~/sayfa-duzenle/{page_id}")]
        public ActionResult EditPage(int page_id)
        {
            var post = _pageM.GetById(page_id);
            ViewBag.PageId = page_id;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult addOrEditPage(PageModel pageModel)
        {
            ResponseModel result = new ResponseModel();

            if (!ModelState.IsValid)
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return Json(result);
            }

            if (pageModel.ID <= 0)
            {
                result = _pageM.AddNewPage(pageModel);
            }
            else
            {
                result = _pageM.UpdatePage(pageModel);
            }
            return Json(result);
        }

        public JsonResult getPageById(int page_id)
        {
            var page = _pageM.GetById(page_id);
            return Json(new ResponseModel { IsSuccess = true, ResultData = page }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult generateUrlFormat(string uri)
        {
            string value = _pageM.GenerateUriFormat(uri);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult updatePublishPage(int page_id, bool p)
        {
            _pageM.UpdatePageIsPublishField(page_id, p);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}