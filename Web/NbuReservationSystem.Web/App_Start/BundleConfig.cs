namespace NbuReservationSystem.Web
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        private const string CssBundleRoot = "~/Content/";
        private const string JsBundleRoot = "~/Scripts/";

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(
                new ScriptBundle("~/bundles/jquery").Include(
                    FormatJsBundlePath("jquery-{version}.js"),
                    FormatJsBundlePath("jquery.validate*"),
                    FormatJsBundlePath("jquery.unobtrusive-ajax.js")
                )
            );

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap-dp").Include(
                    FormatJsBundlePath("moment-with-locales.min.js")
                    //FormatJsBundlePath("bootstrap-datetimepicker.min.js")
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    FormatJsBundlePath("bootstrap.js"),
                    FormatJsBundlePath("bootstrap-select.min.js")
                )
            );
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/Content/css").Include(
                   FormatCssBundlePath("bootstrap.css"),
                   FormatCssBundlePath("bootstrap-theme.css"),
                   FormatCssBundlePath("bootstrap-select.min.css"),
                   FormatCssBundlePath("Site.css")
                )
            );
        }

        private static string FormatJsBundlePath(string jsFile, string root = JsBundleRoot)
            => $"{root}{jsFile}";

        private static string FormatCssBundlePath(string cssFile, string root = CssBundleRoot)
            => $"{root}{cssFile}";
    }
}
