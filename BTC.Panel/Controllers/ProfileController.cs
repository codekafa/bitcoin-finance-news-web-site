using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Panel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    [AuthAttribute(null)]
    public class ProfileController : BaseController
    {

        RegisterManager _regM;
        UserManager _userM;
        LoginManager _loginM;
        public ProfileController()
        {
            _regM = new RegisterManager();
            _userM = new UserManager();
            _loginM = new LoginManager();
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
            if (result.Company != null && result.Company.ID > 0)
            {
                result.CompanyID = result.Company.ID;
                result.CompanyAddress = result.Company.Address;
                result.CompanyCity = result.Company.CityID;
                result.CompanyDescription = result.Company.Description;
                result.CompanyName = result.Company.Name;
                result.CompanyPhone = result.Company.Phone;
            }
            else
            {
                result.Company = new UserCompanies();
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

            if (result.IsSuccess)
            {
                _loginM.ResetSessionUser(userModel.ID);
            }

            return Json(result);
        }


        [Route("~/sifre-degistir")]
        public ActionResult ChangePassword()
        {
            var userModel = SessionVariables.User;
            return View(userModel);
        }

        [HttpPost]
        public JsonResult changePassword(PasswordChangeModel changeModel)
        {
            ResponseModel result = new ResponseModel();
            changeModel.UserID = CurrentUser.CurrentUser.ID;
            result = _regM.ChangePasswordUser(changeModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/cikis-yap")]
        public ActionResult Logout()
        {
            if (SessionVariables.User == null)
                return RedirectToAction("Login", "Security");

            LoginManager _loginM = new LoginManager();
            var result = _loginM.LogoutUser();

            return RedirectToAction("Login", "Security");
        }

    }
}