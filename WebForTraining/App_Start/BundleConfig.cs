using System.Web;
using System.Web.Optimization;

namespace WebForTraining
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Asset/form-validator/jquery.form-validator.js"));

            //bundles.Add(new ScriptBundle("~/bundles/login").Include(
            //           "~/js/sha512.js",
            //           "~/js/forms.js"
            //           ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/js/bootstrap.min.js",
                       "~/js/nicescroll/jquery.nicescroll.min.js",
                       "~/js/notify/pnotify.core.js",
                       "~/js/notify/pnotify.buttons.js",
                       "~/js/notify/pnotify.nonblock.js",
                       "~/js/myScript.js",
                       "~/js/icheck/icheck.min.js",
                        "~/js/custom.js",
                       "~/js/datatables/js/jquery.dataTables.js",
                       "~/js/datepicker/daterangepicker.js",
                       "~/js/datepicker/bootstrap-datepicker.js",
                       "~/js/timepicker/jquery.timepicker.min.js",
                       "~/js/datatables/tools/js/dataTables.tableTools.js"
                      ));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/css/bootstrap.min.css",
                      "~/css/custom.css",
                      "~/fonts/css/font-awesome.min.css",
                      "~/css/animate.min.css",
                      "~/less/custom.css",
                      "~/css/normalize.css",
                      "~/css/icheck/flat/green.css",
                      "~/js/datepicker/datepicker.css",
                      "~/js/timepicker/jquery.timepicker.css",
                      "~/css/datatables/tools/css/dataTables.tableTools.css"
                      ));

            //bundles.Add(new ScriptBundle("~/bundles/scripts").IncludeDirectory("~/Scripts", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
