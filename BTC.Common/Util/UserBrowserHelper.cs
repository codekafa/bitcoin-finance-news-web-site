using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Common.Util
{
    public class UserBrowserHelper
    {
        public static LoginAttempts getUserRequestInfo()
        {
            LoginAttempts la = new LoginAttempts()
            {
                UserIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress,
                CreateDate = DateTime.Now,
                IP4Address = GetIPAddress(),
                MacAddress = GetMACAddress(),
                GeoLocation = null,
                HostName = Environment.MachineName,
                UserAgent = System.Web.HttpContext.Current.Request.UserAgent,
                CalledURL = System.Web.HttpContext.Current.Request.Url.OriginalString,
                Browser = System.Web.HttpContext.Current.Request.Browser.Browser + "/" + System.Web.HttpContext.Current.Request.Browser.Version,
                OpSystem = GetUserPlatform(),
#pragma warning disable CS0618 // Type or member is obsolete
                BrowserJavascript = System.Web.HttpContext.Current.Request.Browser.JavaScript
#pragma warning restore CS0618 // Type or member is obsolete
            };

            return la;
        }

        public static string GetIPAddress()
        {
            string IPAddress = string.Empty;

            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

        }

        public static string _GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static string GetMACAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string address = context.Request.Headers["X-Forwarded-For"];
            if (String.IsNullOrEmpty(address))
                address = context.Request.UserHostAddress;
            return address;
        }

        public static string GetUserEnvironment()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            var browser = context.Request.Browser;
            var platform = GetUserPlatform();
            return string.Format("{0} {1} / {2}", browser.Browser, browser.Version, platform);
        }

        public static string GetUserPlatform()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            var ua = context.Request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            //fallback to basic platform:
            return context.Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        public static string GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
    }
}
