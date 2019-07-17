using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Controllers
{
    [AuthAttribute(null)]
    public class ProfileController : BaseController
    {
        RegisterManager _regM;
        UserManager _userM;
        CitiesManager _cityM;
        public ProfileController()
        {
            _regM = new RegisterManager();
            _userM = new UserManager();
            _cityM = new CitiesManager();
        }

        [Route("~/profilim")]
        public ActionResult MyProfile()
        {
            UpdateUserModel result = new UpdateUserModel();
            result.ID = SessionVariables.User.CurrentUser.ID;
            result.Email = SessionVariables.User.CurrentUser.Email;
            result.Facebook = SessionVariables.User.CurrentUser.Facebook;
            result.FirstName = SessionVariables.User.CurrentUser.FirstName;
            result.Instagram = SessionVariables.User.CurrentUser.Instagram;
            result.LastName = SessionVariables.User.CurrentUser.LastName;
            result.Linkedin = SessionVariables.User.CurrentUser.Linkedin;
            result.Phone = SessionVariables.User.CurrentUser.Phone;
            result.Summary = SessionVariables.User.CurrentUser.Summary;
            result.PhotoName = SessionVariables.User.CurrentUser.ProfilePhotoUrl;
            result.Company = SessionVariables.User.Company;
            if (SessionVariables.User.Company != null)
            {
                ViewBag.CityIDForUser = SessionVariables.User.Company.CityID;
            }


            return View(result);
        }

        [HttpPost]
        public JsonResult updateProfile(UpdateUserModel userModel)
        {
            ResponseModel result = new ResponseModel();
            userModel.ID = SessionVariables.User.CurrentUser.ID;

            if (userModel.ProfilePhotoUrl != null)
            {
                string file_name = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                string base_file_path = WebConfigurationManager.AppSettings["BaseUserFileAddress"];
                string base_file_address = HttpContext.Server.MapPath(base_file_path);
                string savedBaseFilePath = Path.Combine(base_file_address, file_name);
                userModel.PhotoName = file_name;
                userModel.PhotoSaveBaseUrl = savedBaseFilePath;
            }

            result = _userM.UpdateProfileUser(userModel);
            return Json(result);
        }

        [Route("~/sifre-degistir")]
        public ActionResult ChangePassword()
        {
            PasswordChangeModel result = new PasswordChangeModel();
            return View(result);
        }

        [HttpPost]
        public JsonResult changePassword(PasswordChangeModel changeModel)
        {
            ResponseModel result = new ResponseModel();
            changeModel.UserID = CurrentUser.CurrentUser.ID;
            result = _regM.ChangePasswordUser(changeModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCities()
        {
            return Json(_cityM.GetAllCities(), JsonRequestBehavior.AllowGet);
        }
    }
}