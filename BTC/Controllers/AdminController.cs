using BTC.Base;
using BTC.Business.Managers;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BTC.Controllers
{
    [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin })]
    public class AdminController : BaseController
    {
        UserManager _userM;
        PostManager _postM;
        CategoryManager _catM;
        public AdminController()
        {
            _userM = new UserManager();
            _postM = new PostManager();
            _catM = new CategoryManager();
        }

        [Route("~/dashboard")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("~/kullanicilar")]
        public ActionResult Users()
        {
            var user_List = _userM.GetAllUsers();
            return View(user_List);
        }


        [Route("~/makale-listesi")]
        public ActionResult Posts()
        {
            return View();
        }

        public PartialViewResult _GetPosts(int? category_id, int user_id, bool publish)
        {
            List<PostModel> list = new List<PostModel>();
            list = _postM.GetAllPost(category_id, user_id, publish);
            return PartialView(list);
        }

        public JsonResult getWriters()
        {
            var list = _userM.GetSelectedUsers((int)EnumVariables.Roles.Writer);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [Route("~/urun-listesi")]
        public ActionResult Products()
        {
            return View();
        }

        [Route("~/yorum-listesi")]
        public ActionResult Comments()
        {
            return View();
        }

        public JsonResult getWaitingComments()
        {
            List<CommentListModel> commentList = _postM.GetWaitingComments();
            return Json(commentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult publishComment(int comment_id)
        {
            ResponseModel result = new ResponseModel();
            result = _postM.PublishComment(comment_id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult deleteComment(int comment_id)
        {
            ResponseModel result = new ResponseModel();
            result = _postM.DeleteComment(comment_id);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [Route("~/kategori-listesi")]
        public ActionResult Categories()
        {
            return View();
        }

        [HttpPost]
        public JsonResult addOrEditCategory(Categories category)
        {
            ResponseModel result = new ResponseModel();
            if (category.ID > 0)
            {
                category.Uri = _postM.GenerateUriFormat(category.Name);
                result = _catM.UpdateCategory(category);
            }
            else
            {
                category.Uri = _postM.GenerateUriFormat(category.Name);
                category.CreateDate = DateTime.Now;
                result = _catM.AddCategory(category);
            }
            return Json(result);
        }

        public JsonResult updateCategoryState(int id, bool state)
        {
            ResponseModel result = new ResponseModel();

            result = _catM.UpdateCategoryState(id, state);

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [Route("~/mail-ayarlari")]
        public ActionResult MailSettings()
        {
            return View();
        }


        [Route("~/sms-ayarlari")]
        public ActionResult SmsSettings()
        {
            return View();
        }

        [Route("~/site-ayarlari")]
        public ActionResult SiteSettings()
        {
            return View();
        }

    }
}