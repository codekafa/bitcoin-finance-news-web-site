using BTC.Base;
using BTC.Business.Managers;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Controllers
{
    public class NewsController : BaseController
    {
        PostManager _postM;
        CategoryManager _catRepo;

        public NewsController()
        {
            _postM = new PostManager();
            _catRepo = new CategoryManager();
        }

        [Route("~/haberler/son-dakika-haberleri")]
        public ActionResult News()
        {
            return View();
        }

        [Route("~/haberler/son-dakika-haberleri/{news_uri}")]
        public ActionResult NewsDetail(string news_uri)
        {
            var result = _postM.GetPostModelByUri(news_uri);
            _postM.UpdateViewCount(result.ID);
            return View(result);
        }

        public PartialViewResult _GetPartialViewNews()
        {
            var postList = _postM.GetNewsByFilterModel();
            return PartialView(postList);
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.NewsPaper })]

        [Route("~/haberler")]
        public ActionResult GetMyNews()
        {
            return View();
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.NewsPaper })]
        [HttpGet]
        public JsonResult getMyNews()
        {
            var list = _postM.GetNewsModelsByUserId(CurrentUser.CurrentUser.ID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.NewsPaper })]
        [Route("yeni-haber-ekle")]
        public ActionResult AddNews()
        {
            return View();
        }


        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.NewsPaper })]
        [Route("~/haber-duzenle/{news_id}")]
        public ActionResult EditNews(int news_id)
        {
            var post = _postM.GetById(news_id);

            if (post == null || !post.IsActive || post.UserID != CurrentUser.CurrentUser.ID)
            {
                return RedirectToErrorPage(new ResponseModel { IsSuccess = false, Message = "Haber bulunamadı!" });
            }
            ViewBag.NewsId = news_id;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.NewsPaper })]
        public JsonResult addOrEditNews(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            if (!ModelState.IsValid)
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return Json(result);
            }
            postModel.UserID = CurrentUser.CurrentUser.ID;
            postModel.IsNews = true;
            if (postModel.ID <= 0)
            {
                string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string base_file_path = WebConfigurationManager.AppSettings["BasePostFileAddress"];
                string base_file_address = HttpContext.Server.MapPath(base_file_path);
                string savedBaseFilePath = Path.Combine(base_file_address, file_name);
                postModel.TopPhotoUrl = file_name;
                postModel.FileSaveMap = savedBaseFilePath;
                result = _postM.AddNewPost(postModel);
            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    if (postModel.IsChangeMainImage)
                    {
                        string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                        string base_file_path = WebConfigurationManager.AppSettings["BasePostFileAddress"];
                        string base_file_address = HttpContext.Server.MapPath(base_file_path);
                        string savedBaseFilePath = Path.Combine(base_file_address, file_name);
                        postModel.TopPhotoUrl = file_name;
                        postModel.FileSaveMap = savedBaseFilePath;
                        postModel.MainImage = Request.Files[0];
                    }

                }
                result = _postM.UpdatePost(postModel);
            }
            return Json(result);
        }

        public JsonResult getNewsById(int news_id)
        {
            var post = _postM.GetById(news_id);

            if (post == null || !post.IsActive || post.UserID != CurrentUser.CurrentUser.ID)
            {
                return Json(new ResponseModel { IsSuccess = false, Message = "Haber bulunamadı!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new ResponseModel { IsSuccess = true, ResultData = post }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCategories()
        {
            return Json(_catRepo.GetCategories(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult updatePublishNews(int news_id, bool p)
        {
            _postM.UpdatePublishFiledPost(news_id, p);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult generateUrlFormat(string uri)
        {
            string value = _postM.GenerateUriFormat(uri);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}