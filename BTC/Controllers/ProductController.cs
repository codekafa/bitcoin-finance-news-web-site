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
        ImageManager _imgM;
        public ProductController()
        {
            _proM = new ProductManager();
            _imgM = new ImageManager();
        }


        [Route("~/urunlerim")]
        public ActionResult MyProducts()
        {

            return View();
        }

        public PartialViewResult _GetPartialViewMyProducts()
        {
            List<UserProducts> productList = _proM.GetProductByUserID(CurrentUser.CurrentUser.ID);
            return PartialView(productList);
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
            EditProductModel result = new EditProductModel();
            result = _proM.GetEditProductModel(product_id, CurrentUser.CurrentUser.ID);

            if (result == null)
                return RedirectNotAuthority();

            return View(result);
        }


        [HttpPost]
        public JsonResult addPhotos(AddProductPhotosModel addphotoModel)
        {
            ResponseModel result = new ResponseModel();

            if (addphotoModel.PhotoList != null && addphotoModel.PhotoList.Count > 0)
            {
                try
                {
                    foreach (HttpPostedFileWrapper item in addphotoModel.PhotoList)
                    {
                        ProductPhotos photo = new ProductPhotos();
                        photo.ProductID = addphotoModel.ProductID;
                        string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                        string base_file_path = WebConfigurationManager.AppSettings["BaseProductFileAddress"];
                        string base_file_address = HttpContext.Server.MapPath(base_file_path);
                        string base_path = Path.Combine(base_file_address, file_name);
                        _imgM.SaveProductImage(item, base_path);
                        photo.Photo = file_name;
                        photo.IsMain = false;
                        _proM.AddPhotosToProduct(photo);
                    }

                    result.IsSuccess = true;
                    
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = ex.Message;
                }
              
            }
          
            return Json(result);
        }

        public JsonResult removePhoto(int photo_id)
        {
            ResponseModel result = new ResponseModel();

            result = _proM.DeletePhoto(photo_id);

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult updateIsMain(int photo_id,int product_id)
        {
            ResponseModel result = new ResponseModel();

            result = _proM.UpdateIsMain(photo_id, product_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addProduct(AddProductModel addProduct)
        {
            ResponseModel result = new ResponseModel();

            string base_file_path = WebConfigurationManager.AppSettings["BaseProductFileAddress"];
            string base_file_address = HttpContext.Server.MapPath(base_file_path);
            addProduct.UserID = CurrentUser.CurrentUser.ID;
            addProduct.BasePhotoPath = base_file_address;
            result = _proM.AddProduct(addProduct);

            return Json(result);
        }

        [HttpPost]
        public JsonResult editProduct(EditProductModel editProduct)
        {
            return Json(true);
        }

    }
}