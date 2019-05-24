using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using BTC.Panel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var userModel = SessionVariables.User;
            return View(userModel);
        }

        [HttpPost]
        public JsonResult updateProfile(Users userModel)
        {
            ResponseModel result = new ResponseModel();
            result = _userM.UpdateProfileUser(userModel, null);

            if (result.IsSuccess)
                _loginM.ResetSessionUser(userModel.ID);

            return Json(result, JsonRequestBehavior.AllowGet);
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