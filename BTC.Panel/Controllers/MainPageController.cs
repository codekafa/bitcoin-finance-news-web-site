using BTC.Business.Managers;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    public class MainPageController : Controller
    {
        MainPageManager _mainM;
        public MainPageController()
        {
            _mainM = new MainPageManager();
        }

        // GET: MainPage
        [Route("~/slider-ayarlari")]
        public ActionResult Sliders()
        {
            return View();
        }

        public PartialViewResult _GetSliders()
        {
            var list = _mainM.GetMainSliders();
            return PartialView(list);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult addSlider(AddSliderModel slider)
        {
            string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
            string base_file_path = WebConfigurationManager.AppSettings["BaseSliderAddress"];
            string base_file_address = HttpContext.Server.MapPath(base_file_path);
            string savedBaseFilePath = Path.Combine(base_file_address, file_name);
            slider.SaveBaseAddress = savedBaseFilePath;
            slider.PhotoUrl = file_name;
            ResponseModel result = _mainM.CreateSliderItem(slider);
            return Json(result);
        }


        public JsonResult deleteSlider(int slider_id)
        {
            ResponseModel result = new ResponseModel();
            result = _mainM.DeleteSlider(slider_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/ana-sayfa-ayarlari")]
        public ActionResult MainPageSettings()
        {
            var item = _mainM.GetMainPage();
            return View(item);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult saveMainPageSettings(MainPageSettings main)
        {
            ResponseModel result = new ResponseModel();
            result = _mainM.UpdateMainPageSettings(main);
            return Json(result);
        }

    }
}