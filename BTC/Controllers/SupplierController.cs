using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class SupplierController : Controller
    {
        public PartialViewResult _GlobalSupplierMenuItems()
        {


            return PartialView();
        }
        public PartialViewResult _RegionalSupplierMenuItems()
        {
            return PartialView();
        }

        [Route("tedarikci-detaylari/{supplier_name}")]
        public ActionResult GetSupplierDetail(string supplier_name)
        {
            return View();
        }

        public PartialViewResult _GetSupplierDetail(int supplier_id)
        {
            return PartialView();
        }


        [Route("tedarikci-urunleri/{supplier_name}/urunler")]
        public ActionResult GetSupplierDetailProducts(string supplier_name)
        {
            return View();
        }

        public PartialViewResult _GetSupplierProducts(int supplier_id)
        {
            return PartialView();
        }

    }
}