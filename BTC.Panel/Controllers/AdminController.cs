﻿using BTC.Business.Managers;
using BTC.Common.Constants;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Panel.Base;
using BTC.Setting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    [AuthAttribute(new int[] { (int)EnumVariables.Roles.Admin })]
    public class AdminController : BaseController
    {
        UserManager _userM;
        PostManager _postM;
        CategoryManager _catM;
        SiteSettingManager _siteM;
        ContentViewManager _contentM;
        MainMenuManager _menuM;
        ProductManager _proM;
        PagesManager _pageM;
        CitiesManager _cityM;
        public AdminController()
        {
            _userM = new UserManager();
            _postM = new PostManager();
            _catM = new CategoryManager();
            _siteM = new SiteSettingManager();
            _contentM = new ContentViewManager();
            _menuM = new MainMenuManager();
            _proM = new ProductManager();
            _pageM = new PagesManager();
            _cityM = new CitiesManager();

        }

        [Route("~/dashboard")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("~/kullanicilar")]
        public ActionResult Users()
        {
            return View();
        }

        public PartialViewResult _GetUsers(string search_key)
        {
            var user_List = _userM.GetUserListModel(search_key);
            return PartialView(user_List);
        }

        public JsonResult updateUserRole(int user_id, bool state , int operation_id)
        {

            ResponseModel result = new ResponseModel();
            result = _userM.UpdateUserRole(user_id,  state, operation_id);
            return Json(result, JsonRequestBehavior.AllowGet);

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

        [HttpPost]
        public PartialViewResult _GetProducts(ProductSearchModel search)
        {
            List<UserProducts> list = new List<UserProducts>();
            list = _proM.GetProducts(search);
            return PartialView(list);
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


        public JsonResult getSuppliers()
        {
            List<Users> list = new List<Users>();
            list = _userM.GetUserWithRoleId((int)EnumVariables.Roles.Supplier);
            return Json(list, JsonRequestBehavior.AllowGet);
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


        [Route("~/menu-icerikleri")]
        public ActionResult ContentList()
        {
            var list = _contentM.GetAllContentViews();
            return View(list);
        }

        [Route("~/icerik-olustur")]
        [Route("~/icerik-duzenle/{content_id}")]
        public ActionResult AddOrEditContentView(int? content_id)
        {
            ContentViews contentView = new ContentViews();

            if (content_id.HasValue)
                contentView = _contentM.GetContentViewByID(content_id.Value);

            if (contentView == null)
                contentView = new ContentViews();

            return View(contentView);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult addOrEditContent(ContentViews content)
        {
            ResponseModel result = new ResponseModel();

            if (content.ID <= 0)
            {
                result = _contentM.AddNewContent(content);
            }
            else
            {
                result = _contentM.UpdateContent(content);
            }

            return Json(result);
        }

        public JsonResult updateContentStatus(int content_id, bool state, int operation_type)
        {
            ResponseModel result = new ResponseModel();
            switch (operation_type)
            {
                case 1:
                    result = _contentM.UpdateIsPublishContent(content_id, state);
                    break;
                case 2:
                    result = _contentM.UpdateCanSeeMemberContent(content_id, state);
                    break;
                case 3:
                    result = _contentM.UpdateCanSeeTraderContent(content_id, state);
                    break;
                case 4:
                    result = _contentM.UpdateCanSeeUserContent(content_id, state);
                    break;
                case 5:
                    result = _contentM.UpdateCanSeeVipContent(content_id, state);
                    break;
                case 6:
                    result = _contentM.UpdateCanSeeWriterContent(content_id, state);
                    break; ;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/menu-ayarlari")]
        public ActionResult MainMenu()
        {
            return View();
        }

        public JsonResult getParentMenuItems()
        {
            return Json(_menuM.GetParentMenuItems(), JsonRequestBehavior.AllowGet);
        }

        [Route("~/alt-menu-ekle/{menu_id}")]
        public ActionResult AddSubMenu(int menu_id)
        {
            ViewBag.MenuID = menu_id;
            return View();
        }

        public PartialViewResult _GetSubMenus(int menu_id)
        {
            var list = _menuM.GetSubMenuItems(menu_id);
            ViewBag.MenuID = menu_id;
            return PartialView(list);
        }

        public PartialViewResult _GetStaticPages()
        {
            var list = _menuM.GetStaticPages();
            return PartialView(list);
        }

        [HttpPost]
        public JsonResult addOrEditSubMenu(AddOrEditSubMenuModel menuItem)
        {
            ResponseModel result = new ResponseModel();

            result = _menuM.AddOrEditSubMenu(menuItem);

            return Json(result);
        }

        public JsonResult generateUriFormat(string title)
        {
            return Json(_postM.GenerateUriFormat(title), JsonRequestBehavior.AllowGet);
        }

        public JsonResult addPageMenuItem(int menu_id, int page_id, int row_number, int type_id)
        {
            ResponseModel result = new ResponseModel();

            result = _menuM.AddSubPageToMenu(menu_id, page_id, row_number, type_id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult addStaticPageMenu(int menu_id, int page_id, int row_number)
        {
            ResponseModel result = new ResponseModel();

            result = _menuM.AddStaticPageToMenu(menu_id, page_id, row_number);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addOrEditMenu(MainMenu menu)
        {
            ResponseModel result = new ResponseModel();
            menu.Url = _postM.GenerateUriFormat(menu.Title);
            menu.IsActive = true;
            if (menu.ID > 0)
            {
                result = _menuM.UpdateMenu(menu);
            }
            else
            {
                menu.IsActive = true;
                result = _menuM.AddMenu(menu);
            }
            return Json(result);
        }

        public JsonResult updateMenuState(int id, bool state)
        {
            ResponseModel result = new ResponseModel();

            result = _menuM.UpdateMenuState(id, state);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult deleteSubMenuItem(int menu_id)
        {
            ResponseModel result = new ResponseModel();

            result = _menuM.DeleteSubMenuItem(menu_id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetPageUrls(int type_id)
        {
            List<PageUrlItem> list = new List<PageUrlItem>();
            list = _menuM.GetPageUrl(type_id);
            ViewBag.TypeID = type_id;
            return PartialView(list);
        }


        [Route("sehirler")]
        public ActionResult Cities()
        {
            var list = _cityM.GetAllCities();
            return View(list);
        }

        [HttpPost]
        public JsonResult addOrEditCity(Cities city)
        {
            ResponseModel result = new ResponseModel();

            if (string.IsNullOrEmpty(city.Uri))
            {
                city.Uri = _postM.GenerateUriFormat(city.Name);
            }

            if (city.ID <= 0)
            {
                result = _cityM.AddCity(city);
            }
            else
            {
                result = _cityM.Update(city);
            }
            return Json(result);
        }

        public JsonResult deleteCity(int city_id)
        {
            ResponseModel result = new ResponseModel();
            result = _cityM.Remove(city_id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCities()
        {
            return Json(_cityM.GetAllCities(),JsonRequestBehavior.AllowGet);
        }


    }
}