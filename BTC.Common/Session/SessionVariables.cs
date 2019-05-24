using BTC.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Common.Session
{
    public class SessionVariables
    {

        #region Internal
        internal static void SetSessionVariable(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null) HttpContext.Current.Session[key] = value;
        }
        internal static T GetSessionVariable<T>(string key)
        {
            T rVal = default(T);
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null) rVal = (T)HttpContext.Current.Session[key];
            return rVal;
        }
        #endregion

        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
        }

        public static void SetUser(CurrentUserModel user)
        {
            if (user != null)
            {
                SetSessionVariable("CurrentUser", user);
            }
        }

        public static CurrentUserModel User
        {
            get
            {
                CurrentUserModel u = null;
                if (HttpContext.Current.Session["CurrentUser"] != null)
                {
                    u = GetSessionVariable<CurrentUserModel>("CurrentUser");
                }

                return u;
            }
        }

    }
}
