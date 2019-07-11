using BTC.Base;
using BTC.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class SupplierController : BaseController
    {
        MainMenuManager _menuM;
        CitiesManager _cityM;
        SupplierManager _supM;
        public SupplierController()
        {
            _menuM = new MainMenuManager();
            _cityM = new CitiesManager();
            _supM = new SupplierManager();

        }
        public PartialViewResult _GlobalSupplierMenuItems()
        {
            var list = _menuM.GetCitiesMenuItems(false);
            return PartialView(list);
        }
        public PartialViewResult _RegionalSupplierMenuItems()
        {
            var list = _menuM.GetCitiesMenuItems(true);
            return PartialView(list);
        }


        [Route("tedarikciler/{source}/{city_name}")]
        public ActionResult GetSupplierByCity(string city_name)
        {
            var city = _cityM.GetCityUri(city_name);

            if (city == null)
            {
                return RedirectToErrorPage(new Model.Response.ResponseModel { IsSuccess = false, Message = "Şehir bulunamadı!" });
            }
            var list = _supM.GetSuppliersByCityID(city.ID);
            ViewBag.CityName = city.Name;
            return View(list);
        }

        [Route("tedarikci-detaylari/{supplier_name}")]
        public ActionResult GetSupplierDetail(string supplier_name)
        {
            var supplier = _supM.GetSupplierByUri(supplier_name);
            return View(supplier);
        }

        public PartialViewResult _GetSupplierDetail(int supplier_id)
        {
            return PartialView();
        }


 

    }
}