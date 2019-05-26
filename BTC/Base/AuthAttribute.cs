using BTC.Common.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BTC.Base
{
    public class AuthAttribute : ActionFilterAttribute, IActionFilter
    {
        private int[] r;
        public AuthAttribute(int[] roleList)
        {
            r = roleList;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isLogin = false;

            if (HttpContext.Current.Session["CurrentUser"] != null)
            {
                isLogin = true;
            }

            if (!isLogin)
            {
                SessionVariables.RemoveAll();
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "Security",
                    area = ""
                }));
            }
            bool auth = false;

            if (isLogin && r != null && r.Count() > 0)
            {
                foreach (var role in r)
                {
                    auth = SessionVariables.User.Roles.Where(x => x.RoleID == role).Count() > 0;
                    if (auth)
                    {
                        break;
                    }
                }
                if (!auth)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "NotAuthority",
                        controller = "Home",
                        area = ""
                    }));
                }
            }

        }
    }
}