using BTC.Common.Session;
using BTC.Model.Response;
using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTC.Panel.Base
{
    public class BaseController : Controller
    {
        public CurrentUserModel CurrentUser { get { return SessionVariables.User; } }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            ResponseModel responseModel = new ResponseModel();
            responseModel.IsSuccess = false;
            responseModel.Message = "Yolunda gitmeyen birşeyler var! Detay: " + filterContext.Exception.Message;
            TempData["ResponseModel"] = responseModel;
            filterContext.Result = RedirectToAction("ErrorPage", "Home");
        }
        public RedirectToRouteResult RedirectToErrorPage(ResponseModel result)
        {
            TempData["ResponseModel"] = result;
            return RedirectToAction("ErrorPage", "Home", result);
        }

    }
}