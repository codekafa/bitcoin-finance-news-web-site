using BTC.Business.Managers;
using BTC.Common.Constants;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Panel.Base;
using BTC.Setting;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    [AuthAttribute(null)]
    public class SiteSettingController : Controller
    {
        SiteSettingManager _siteM;

        public SiteSettingController()
        {
            _siteM = new SiteSettingManager();
        }

        [Route("~/site-ayarlari")]
        public ActionResult SiteSettings()
        {
            var settingModel = StaticSettings.SiteSettings;
            return View(settingModel);
        }

        [HttpPost]
        public JsonResult updateSiteSettings(SiteSettings settingModel)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {
                result = _siteM.UpdateSiteSettings(settingModel);
            }
            else
            {
                result.Message = "Lütfen zorunlu alanları doldurun!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Route("~/mail-ayarlari")]
        public ActionResult EmailSettings()
        {
            var settingModel = StaticSettings.MailSettings;
            return View(settingModel);
        }

        [HttpPost]
        public JsonResult updateMailSettings(MailSettings settingModel)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {
                result = _siteM.UpdateMailSettings(settingModel);
            }
            else
            {
                result.Message = "Lütfen zorunlu alanları doldurun!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/sms-ayarlari")]
        public ActionResult SmsSettings()
        {
            var settingModel = StaticSettings.SmsSettings;
            return View(settingModel);
        }

        [HttpPost]
        public JsonResult updateSmsSettings(SmsSettings settingModel)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {
                result = _siteM.UpdateSmsSettings(settingModel);
            }
            else
            {
                result.Message = "Lütfen zorunlu alanları doldurun!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/uyelik-ayarlari")]
        public ActionResult RegisterSettings()
        {
            return View();
        }
    }
}