using System.Web;
using System.Web.Optimization;

namespace BTC.Panel
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

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/bundles/css/toastr").Include(
                "~/Content/Plugins/toastr/css/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/js/toastr").Include(
            "~/Content/Plugins/toastr/js/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css/vendor").Include(
                    "~/Content/vendors/bootstrap/dist/css/bootstrap.min.css",
                     "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap.theme.min.css",
                    "~/Content/vendors/font-awesome/css/font-awesome.min.css",
                    "~/Content/vendors/themify-icons/css/themify-icons.css",
                    "~/Content/vendors/flag-icon-css/css/flag-icon.min.css",
                    "~/Content/vendors/selectFX/css/cs-skin-elastic.css",
                    "~/Content/vendors/jqvmap/dist/jqvmap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/themescript").Include(
                    "~/Content/vendors/popper.js/dist/umd/popper.min.js",
                    "~/Content/vendors/bootstrap/dist/js/bootstrap.min.js",
                    "~/Content/assets/js/main.js",
                    "~/Content/assets/js/dashboard.js",
                    "~/Content/assets/js/widgets.js",
                    "~/Content/vendors/jqvmap/dist/jquery.vmap.min.js",
                    "~/Content/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                    "~/Content/vendors/jqvmap/dist/maps/jquery.vmap.world.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/user-operation").Include(
             "~/Scripts/Application/UserOperation/user-operation.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/global").Include(
            "~/Scripts/Application/Global/app-global.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/site-setting").Include(
         "~/Scripts/Application/SettingOperation/site-setting.js"));
        }
    }
}
