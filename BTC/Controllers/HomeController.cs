using BTC.Base;
using BTC.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class HomeController : BaseController
    {
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
    }
}