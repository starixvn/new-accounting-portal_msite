namespace Fanex.Maintenance.Site
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    public static class BundleConfig
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            // css
            bundles.AddAndInclude(
                "~/css/common",
                "~/themes/reset.css",
                "~/themes/fonts.css",
                "~/themes/jquery-ui/smoothness/jquery-ui-1.8.23.min.css",
                "~/themes/assets/css/prettify.css",
                "~/themes/assets/css/site.css",
                "~/scripts/lib/tipsy/tipsy-1.0.0.min.css",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.css",
                "~/scripts/lib/chosen/chosen.css");

            bundles.AddAndInclude(
                "~/css/mail-list",
                "~/themes/manage/manage.less",
                "~/themes/manage/RefreshCountDown.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            bundles.AddAndInclude(
                "~/css/manage",
                "~/themes/manage/manage.less");

            bundles.AddAndInclude(
                "~/css/letstalk-notification",
                "~/themes/manage/LetstalkNotification.less");

            bundles.AddAndInclude(
                "~/css/database-report",
                "~/themes/manage/report/dataintegry.less");

            bundles.AddAndInclude(
                "~/css/log",
                "~/themes/manage/log/manage.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css",
                "~/scripts/lib/Select2/select2.css",
                "~/scripts/lib/DatetimePicker/jquery.datetimepicker.css",
                "~/scripts/lib/Select2/select2-custom.css");

            bundles.AddAndInclude(
                "~/css/xml-editor",
                "~/themes/jquery-ui/XMLEditor/smoothness/jquery-ui-1.8.23.css",
                "~/scripts/lib/XMLEditor/css/main.css",
                "~/scripts/lib/XMLEditor/js/ext/GLR/css/GLR.messenger.css");

            bundles.AddAndInclude(
                "~/css/mail-manage",
                "~/themes/manage/log/manage.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            bundles.AddAndInclude(
                "~/css/mail-report",
                "~/themes/manage/log/manage.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            bundles.AddAndInclude(
                "~/css/manage-letstalk-message",
                "~/themes/manage/ManageLetstalkMessage.less");

            bundles.AddAndInclude(
                "~/css/mail-config-log",
                "~/themes/manage/log/mail-config.less",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");

            // js
            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                "~/scripts/lib/jquery-1.8.0.min.js",
                "~/scripts/lib/jquery-ui-1.8.23.min.js",
                "~/scripts/lib/tipsy/tipsy-1.0.0.min.js",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.js",
                "~/scripts/Maintenance.js",
                "~/scripts/lib/chosen/chosen.jquery.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.js",
                "~/scripts/lib/jquery.cookie.js",
                "~/scripts/lib/Fanex.Grid/moment.min.js",
                "~/scripts/lib/moment/moment-timezone-with-data.js",
                "~/scripts/lib/RefreshCountDown.js"));

            bundles.Add(new ScriptBundle("~/scripts/mail-config").Include(
                "~/scripts/Mail/MailConfig.js"));

            bundles.Add(new ScriptBundle("~/scripts/get-mail-config-logs").Include(
                "~/scripts/Mail/GetMailConfigLogs.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-config").Include(
                "~/scripts/Mail/AddMailConfig.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-config-popup").Include(
                "~/scripts/Mail/AddMailConfigPopUp.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-setting-popup").Include(
                "~/scripts/Mail/AddMailSettingPopUp.js"));

            bundles.Add(new ScriptBundle("~/scripts/add-mail-function").Include(
                "~/scripts/Mail/AddMailFunction.js"));

            bundles.Add(new ScriptBundle("~/scripts/xml-editor").Include(
                "~/scripts/lib/jquery-1.8.0.min.js",
                "~/scripts/lib/jquery-ui-1.8.23.min.js",
                "~/scripts/lib/msgbox/msgbox-0.0.1.min.js",
                "~/scripts/Maintenance.js",
                "~/scripts/lib/XMLEditor/js/loc/xmlEditor.js",
                "~/scripts/lib/XMLEditor/js/ext/GLR/GLR.js",
                "~/scripts/lib/XMLEditor/js/ext/GLR/GLR.messenger.js"));

            bundles.Add(new ScriptBundle("~/scripts/letstalk-notification").Include(
                "~/scripts/LetstalkNotification/LetstalkNotification.js"));

            RegisterSharedBundles(bundles);
            RegisterFanexGridBundles(bundles);
            bundles.FileSetOrderList.Clear();
        }

        public static void RegisterSharedBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/sharedscripts").Include(
                "~/scripts/ShareScripts/SharedScriptManager.js"));
        }

        public static void RegisterFanexGridBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/fanexgridscripts").Include(
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.js",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.js"));
            bundles.AddAndInclude(
                "~/themes/fanexgridstyles",
                "~/scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/scripts/lib/Fanex.Grid/Fanex.Pagination.css");
        }
    }
}
