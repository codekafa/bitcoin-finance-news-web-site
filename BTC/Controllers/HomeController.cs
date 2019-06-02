using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Constants;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class HomeController : BaseController
    {
      
        public HomeController()
        {
    
        }

        [Route("~/")]
        [Route("~/ana-sayfa")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("~/hakkimizda")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("~/iletisim")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("~/bir-sorun-var")]
        public ActionResult ErrorPage()
        {
            ResponseModel result = TempData["ResponseModel"] as ResponseModel;

            if (result == null)
            {
                result = new ResponseModel();
                result.IsSuccess = false;
                result.Message = "Birşeyler ters gitti. Bunun için ilgili birimleri bilgilendirdik.İşlemlerinize devam edebilirsiniz.";
            }

            return View(result);
        }

        [Route("~/yetkiniz-yok")]
        public ActionResult NotAuthority()
        {
            ResponseModel result = new ResponseModel();
            result.IsSuccess = false;
            result.Message = "Bu işlem için yetkiniz bulunmamaktadır!";
            return View(result);
        }

        [HttpPost]
        public JsonResult sendContactMessage(ContactModel msj)
        {
            ResponseModel result = new ResponseModel();
            result = msj.IsValid();
            if (result.IsSuccess)
            {
                EmailManager _emailM = new EmailManager();
                result = _emailM.SendMail("İletişim Talebi", new string[] { StaticSettings.SiteSettings.ComtactEmail }, msj.ToString());
            }
            return Json(result);
           
        }
    }
}