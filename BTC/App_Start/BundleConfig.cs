using System.Web;
using System.Web.Optimization;

namespace BTC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                     "~/Scripts/theme/bootstrap.min.js",
                     "~/Scripts/theme/Chart.min.js",
                     "~/Scripts/theme/owncarousel/owl.carousel.min.js",
                     "~/Scripts/theme/popper.min.js", 
                     "~/Scripts/theme/jquery.mCustomScrollbar.concat.min.js", 
                     "~/Scripts/theme/modal/classie.js", 
                     "~/Scripts/theme/modal/modalEffects.js", 
                     "~/Scripts/theme/jquery.waypoints.min.js",
                     "~/Scripts/theme/main.js",
                     "~/Scripts/theme/api.js",
                     "~/Content/Plugins/phoneFormat/jquery-input-mask-phone-number.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/font-awesome-5.5.0/css/all.css",
                      "~/Content/jquery.mCustomScrollbar.min.css",
                      "~/Content/owl.theme.default.css",
                      "~/Content/owl.carousel.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/user-operation").Include(
           "~/Scripts/Application/UserOperation/user-operation.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/global").Include(
         "~/Scripts/Application/Global/app-global.js"));


            bundles.Add(new StyleBundle("~/bundles/css/toastr").Include(
            "~/Content/Plugins/toastr/css/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/toastr").Include(
            "~/Content/Plugins/toastr/js/toastr.min.js"));
        }
    }
}
