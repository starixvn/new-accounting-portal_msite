namespace Fanex.Tool.TraceSP.Services
{
    using System.Configuration;

    internal static class AppSettingService
    {
        public static string DbListXMLPath => ConfigurationManager.AppSettings["DbListXMLPath"];

        public static string SiteXMLPath => ConfigurationManager.AppSettings["SiteXMLPath"];
    }
}
