using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Common.Util.Extension;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Controllers
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
        // GET: Blog
        [Route("~/blog")]
        [Route("~/blog/kategori/{category_name}")]
        [Route("~/blog/etiket/{tag}")]
        [Route("~/blog/search/{search_key}")]
        public ActionResult Blog(string category_name, string search_key, string tag)
        {
            PostFilterModel filter = new PostFilterModel();
            filter.CategoryName = category_name;
            filter.SearchKey = search_key;
            filter.TagName = tag;
            ViewBag.Filter = filter;
            return View();
        }

        [Route("~/blog-detay/{post_uri}")]
        [Route("~/blog-detay/{category}/{post_uri}")]
        public ActionResult BlogDetail(string category, string post_uri)
        {
            var result = _postM.GetPostModelByUri(post_uri);
            _postM.UpdateViewCount(result.ID);
            return View(result);
        }


        public PartialViewResult _GetPartialViewPosts(PostFilterModel filter)
        {
            var postList = _postM.GetPostsByFilterModel(filter);
            return PartialView(postList);
        }

        public PartialViewResult _GetPartialViewBestViewPosts()
        {
            var postList = _postM.GetBestViewTop3Post();
            return PartialView(postList);
        }

        public PartialViewResult _GetCategories()
        {
            var cateList = _catRepo.GetCategories().Where(x => x.IsActive == true).ToList();
            return PartialView(cateList);
        }

        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]

        [Route("~/makalelerim")]
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
        [Route("yeni-makale-ekle")]
        public ActionResult AddNewPost()
        {
            return View();
        }


        [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin, (int)EnumVariables.Roles.Writer })]
        [Route("~/makale-duzenle/{post_id}")]
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

        [HttpPost]
        public JsonResult addComment(Comments addComment)
        {
            ResponseModel result = new ResponseModel();
            addComment.IsPublish = false;
            addComment.CreateDate = DateTime.Now;
            if (SessionVariables.User != null)
            {
                addComment.UserID = SessionVariables.User.CurrentUser.ID;
            }
            result = _postM.AddComment(addComment);
            return Json(result);
        }
    }
}