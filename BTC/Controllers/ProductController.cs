using BTC.Base;
using BTC.Business.Managers;
using BTC.Model.Entity;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class ProductController : BaseController
    {
        ProductManager _proM;
        public ProductController()
        {
            _proM = new ProductManager();
        }


        [Route("~/urunlerim")]
        public ActionResult MyProducts()
        {
            List<UserProducts> productList = _proM.GetProductByUserID(CurrentUser.CurrentUser.ID);
            return View(productList);
        }
        [Route("~/yeni-urun-ekle")]
        public ActionResult AddNewProduct()
        {
            AddProductModel createProduct = new AddProductModel();
            return View(createProduct);
        }

        [Route("~/urun-duzenle/{product_id}")]
        public ActionResult EditProduct(int product_id)
        {
            UserProducts productList = _proM.GetProductByID(product_id, CurrentUser.CurrentUser.ID);
            return View(productList);
        }

    }
}