using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Entity;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    [AuthAttribute(null)]
    public class ProfileController : BaseController
    {
        RegisterManager _regM;
        UserManager _userM;
        public ProfileController()
        {
            _regM = new RegisterManager();
            _userM = new UserManager();
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
            result.ProfilePhotoUrl = SessionVariables.User.CurrentUser.ProfilePhotoUrl;
            result.Summary = SessionVariables.User.CurrentUser.Summary;
            return View(result);
        }

        [AuthAttribute(null)]
        [HttpPost]
        public JsonResult updateProfile(UpdateUserModel userModel)
        {
            ResponseModel result = new ResponseModel();
            userModel.ID = SessionVariables.User.CurrentUser.ID;
            result = _userM.UpdateProfileUser(userModel, null);
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


    }
}