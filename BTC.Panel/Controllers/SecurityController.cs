using BTC.Business.Managers;
using BTC.Common.Session;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Panel.Controllers
{
    public class SecurityController : Controller
    {

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
    }
}