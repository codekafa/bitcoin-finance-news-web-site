using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Constants;
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
        SiteSettingManager _siteM;
        public AdminController()
        {
            _userM = new UserManager();
            _postM = new PostManager();
            _catM = new CategoryManager();
            _siteM = new SiteSettingManager();
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


        public JsonResult updateUserStatus(int user_id, bool state, int operation_type)
        {
            ResponseModel result = new ResponseModel();
            switch (operation_type)
            {
                case 1:
                    result = _userM.UpdateUserIsActiveField(user_id, state);
                    break;
                case 2:
                    result = _userM.UpdateUserIsVipField(user_id, state);
                    break;
                case 3:
                    result = _userM.UpdateUserIsApproveField(user_id, state);
                    break;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult updatePostStatus(int category_id, bool state, int operation_type)
        {
            ResponseModel result = new ResponseModel();
            switch (operation_type)
            {
                case 1:
                    result = _postM.UpdatePostIsActiveField(category_id, state);
                    break;
                case 2:
                    result = _postM.UpdatePostIsPublishField(category_id, state);
                    break;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
                category.Uri = _postM.GenerateUriFormat(category.Uri);
                result = _catM.UpdateCategory(category);
            }
            else
            {
                category.Uri = _postM.GenerateUriFormat(category.Uri);
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
            var settingModel = StaticSettings.MailSettings;
            return View(settingModel);
        }

        [HttpPost]
        public JsonResult updateMailSettings(MailSettings settingModel)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {    
                result = _siteM.UpdateMailSettings(settingModel);
            }
            else
            {
                result.Message = "Lütfen zorunlu alanları doldurun!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/sms-ayarlari")]
        public ActionResult SmsSettings()
        {
            return View();
        }

        [Route("~/site-ayarlari")]
        public ActionResult SiteSettings()
        {
            var settingModel = StaticSettings.SiteSettings;
            return View(settingModel);
        }

        [HttpPost]
        public JsonResult updateSiteettings(SiteSettings settingModel)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {
                result = _siteM.UpdateSiteSettings(settingModel);
            }
            else
            {
                result.Message = "Lütfen zorunlu alanları doldurun!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}