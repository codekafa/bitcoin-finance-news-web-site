using BTC.Base;
using BTC.Business.Managers;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Controllers
{
    [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Supplier })]
    public class ProductController : BaseController
    {
        ProductManager _proM;
        public ProductController()
        {
            _proM = new ProductManager();
            //_imgM = new ImageManager();
        }


        [Route("~/urun-listesi")]
        public ActionResult Products()
        {
            return View();
        }

        public PartialViewResult _GetPartialViewProducts(int supplier_id)
        {
            List<UserProducts> productList = _proM.GetProductByUserID(supplier_id);
            return PartialView(productList);
        }

        [Route("~/urun-detay/{productUri}")]
        public ActionResult ProductDetails(string productUri)
        {
            UserProducts item = _proM.GetProductByUri(productUri);
            return View();
        }


    }
}