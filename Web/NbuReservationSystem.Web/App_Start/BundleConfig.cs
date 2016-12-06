namespace NbuReservationSystem.Web
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        private const string CssBundleRoot = "~/Content/";
        private const string JsBundleRoot = "~/Scripts/";

        private const string Bootstrap28CssRoot = "~/Content/bootstrap-28/";
        private const string Bootstrap28JsRoot = "~/Scripts/bootstrap-28/";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(FormatJsBundlePath("jquery-{version}.js")));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(FormatJsBundlePath("jquery.validate*")));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryun").Include(FormatJsBundlePath("jquery.validate.unobtrusive.js")));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js",
            "~/Scripts/jquery.validate*",
            "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(FormatJsBundlePath("bootstrap.js")));
            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(FormatJsBundlePath("bootstrap.js", Bootstrap28JsRoot)));
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Content/css").Include(
                   FormatCssBundlePath("bootstrap.css"),
                   FormatCssBundlePath("bootstrap-theme.css"),
                   FormatCssBundlePath("Site.css")
                )
            );
            //bundles.Add(
            //    new StyleBundle("~/Content/bootstrap-28").Include(
            //        FormatCssBundlePath("bootstrap.css", Bootstrap28CssRoot),
            //        FormatCssBundlePath("bootstrap-theme.css", Bootstrap28CssRoot),
            //        FormatCssBundlePath("Site.css", Bootstrap28CssRoot)
            //    )
            //);
        }

        private static string FormatJsBundlePath(string jsFile, string root = JsBundleRoot)
            => $"{root}{jsFile}";

        private static string FormatCssBundlePath(string cssFile, string root = CssBundleRoot)
            => $"{root}{cssFile}";
    }
}
