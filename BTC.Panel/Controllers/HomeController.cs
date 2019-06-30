using BTC.Business.Managers;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Panel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    [AuthAttribute(null)]
    public class HomeController : Controller
    {
        private SiteSettingManager _siteM; 
        public HomeController()
        {
            _siteM = new SiteSettingManager();
        }

        [Route("~/")]
        [Route("~/ana-sayfa")]
        public ActionResult Index()
        {
            var result = _siteM.GetDashboardModel();
            return View(result.ResultData as DasboardMainModel);
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