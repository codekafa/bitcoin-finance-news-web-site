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
    public class ProductController : BaseController
    {
        ProductManager _proM;
        SupplierManager _supM;
        public ProductController()
        {
            _proM = new ProductManager();
            _supM = new SupplierManager();
        }


        [Route("tedarikci-urunleri/{supplier_name}/urunler")]
        public ActionResult GetSupplierDetailProducts(string supplier_name)
        {
            var supplier = _supM.GetSupplierByUri(supplier_name);

            if (supplier == null)
            {
                RedirectToErrorPage(new ResponseModel { IsSuccess = false, Message = "Tedarikçi bulunamadı!" });
            }

            _proM.UpdateProductViews(supplier.UserID);
            return View(supplier);
        }

        public PartialViewResult _GetSupplierProducts(int supplier_id)
        {
            var list = _proM.GetProductByUserID(supplier_id);
            return PartialView(list);
        }

        [Route("urun-detay/{product_uri}")]
        public ActionResult GetProductDetailByUri(string product_uri)
        {
            var product = _proM.GetProductByUri(product_uri);
            return View(product);
        }



    }
}