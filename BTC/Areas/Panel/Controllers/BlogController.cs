using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Areas.Panel.Controllers
{
    public class BlogController : BaseController
    {
        PostManager _postM;
        CategoryManager _catRepo;

        public BlogController()
        {
            _postM = new PostManager();
            _catRepo = new CategoryManager();
        }


        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]

        [Route("panel/makalelerim")]
        public ActionResult GetMyPosts()
        {
            return View();
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]
        [HttpGet]
        public JsonResult getMyPosts()
        {
            var list = _postM.GetPostModelsByUserId(CurrentUser.CurrentUser.ID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]
        [Route("panel/yeni-makale-ekle")]
        public ActionResult AddNewPost()
        {
            return View();
        }


        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]
        [Route("panel/makale-duzenle/{post_id}")]
        public ActionResult EditPost(int post_id)
        {
            var post = _postM.GetById(post_id);

            if (post == null || !post.IsActive || post.UserID != CurrentUser.CurrentUser.ID)
            {
                return RedirectToErrorPage(new ResponseModel { IsSuccess = false, Message = "Makale bulunamadı!" });
            }
            ViewBag.PostId = post_id;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]
        public JsonResult addOrEditPost(PostModel postModel)
        {
            ResponseModel result = new ResponseModel();

            if (!ModelState.IsValid)
            {
                result.Message = "Zorunlu alanları doldurunuz!";
                return Json(result);
            }
            postModel.UserID = CurrentUser.CurrentUser.ID;

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

        public JsonResult getPostById(int post_id)
        {
            var post = _postM.GetById(post_id);

            if (post == null || !post.IsActive || post.UserID != CurrentUser.CurrentUser.ID)
            {
                return Json(new ResponseModel { IsSuccess = false, Message = "Makale bulunamadı!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new ResponseModel { IsSuccess = true, ResultData = post }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCategories()
        {
            return Json(_catRepo.GetCategories(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult updatePublishPost(int post_id, bool p)
        {
            _postM.UpdatePublishFiledPost(post_id, p);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult generateUrlFormat(string uri)
        {
            string value = _postM.GenerateUriFormat(uri);
            return Json(value, JsonRequestBehavior.AllowGet);
        }

    }
}