using BTC.Common.Session;
using BTC.Model.Response;
using BTC.Model.View;
using System.Web.Mvc;

namespace BTC.Base
{
    public class BaseController : Controller
    {
        public CurrentUserModel CurrentUser { get { return SessionVariables.User; } }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            ResponseModel responseModel = new ResponseModel();
            responseModel.IsSuccess = false;
            responseModel.Message = "Yolunda gitmeyen birşeyler var!";
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