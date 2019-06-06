using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace BTC.Common.Constants
{
    public  static class ConstantProxy
    {
        public static string BaseProfilePhotoPath { get {return WebConfigurationManager.AppSettings["BaseProfilePhotoPath"].ToString(); } }

        public static string BaseImagePath { get { return WebConfigurationManager.AppSettings["BaseImagePath"].ToString(); } }
        public static string CategoryCacheName { get { return "categories"; } }

        public static string BaseSiteUrl { get { return WebConfigurationManager.AppSettings["BaseSiteUrl"].ToString(); } }

    }
}
