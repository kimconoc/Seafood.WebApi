using System.Web;
using System.Web.Optimization;

namespace Amin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        //"~/Scripts/jquery/jquery-{version}.js",
                        //"~/assets/vendors/js/vendor.bundle.base.js",
                        "~/assets/js/material.js",
                        "~/assets/js/dashboard.js",
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/bootstrap/bootstrap.js",
                        "~/Scripts/jquery-confirm.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.material.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                    "~/assets/vendors/mdi/css/materialdesignicons.min.css",
                    "~/assets/vendors/css/vendor.bundle.base.css",
                    "~/Content/jquery-confirm.css",
                    "~/Content/dataTables.material.min.css",
                    "~/Content/bootstrap.css",
                    "~/Content/material.min.css",
                    "~/Content/docs.css"));
        }
    }
}
