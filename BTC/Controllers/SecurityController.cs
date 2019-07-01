using BTC.Base;
using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Controllers
{
    [RoutePrefix("uyelik")]
    public class SecurityController : BaseController
    {
        private RegisterManager _regM;
        public SecurityController()
        {
            _regM = new RegisterManager();
        }

        [Route("~/yeni-uyelik-olustur")]
        public ActionResult Register()
        {
            if (SessionVariables.User != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public JsonResult registerUser(RegisterUserModel registerUser)
        {
            ResponseModel result = new ResponseModel();
            if (ModelState.IsValid)
            {
                result = _regM.RegisterUser(registerUser);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Zorunlu alanları doldurunuz!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/giris-yap")]
        public ActionResult Login()
        {

            if (SessionVariables.User != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public JsonResult loginUser(LoginUserModel loginUser)
        {
            if (ModelState.IsValid)
            {
                LoginManager _loginM = new LoginManager();
                var result = _loginM.LoginUser(loginUser);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResponseModel { IsSuccess = false, Message = "Bilgileri kontrol ediniz!" });
            }
        }

        [Route("~/uyelik/dogrulama/{guid}")]
        public ActionResult ApproveUser(string guid)
        {
            if (SessionVariables.User != null)
                return RedirectToAction("Index", "Home");

            ResponseModel result = new ResponseModel();
            result = _regM.ApproveUserMailGuid(guid);

            if (result.IsSuccess)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToErrorPage(result);
        }

        public JsonResult logoutUser()
        {
            LoginManager _loginM = new LoginManager();
            var result = _loginM.LogoutUser();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("~/cikis-yap")]
        public ActionResult Logout()
        {
            if (SessionVariables.User == null)
                return RedirectToAction("Index", "Home");

            LoginManager _loginM = new LoginManager();
            var result = _loginM.LogoutUser();

            return RedirectToAction("Index", "Home");
        }

        [Route("~/sifremi-unuttum")]
        public ActionResult ForgatPassword()
        {
            if (SessionVariables.User != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public JsonResult sendChangePasswordMail(string email)
        {
            ResponseModel resultModel = new ResponseModel();
            resultModel = _regM.SendChangePasswordMailGuid(email);
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        [Route("~/sifre-sifirlama/{guid}")]
        public ActionResult ChangePassword(string guid)
        {
            if (SessionVariables.User != null)
                return RedirectToAction("Index", "Home");

            var result = _regM.ValidateChangePasswordUserByGuid(guid);

            if (!result.IsSuccess)
                return RedirectToErrorPage(result);

            ViewBag.Guid = guid;
            return View();
        }

        [HttpPost]
        public JsonResult changePassword(PasswordChangeModel changeModel)
        {
            ResponseModel result = new ResponseModel();
            result = _regM.ChangeForgatPasswordUser(changeModel);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}