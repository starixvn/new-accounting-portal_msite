namespace Fanex.Tool.TraceSP.Services
{
    using System.Configuration;
    static class AppSettingService
    {
        public static string DbListXMLPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DbListXMLPath"];
            }
        }

        public static string SiteXMLPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteXMLPath"];
            }
        }
    }
}
