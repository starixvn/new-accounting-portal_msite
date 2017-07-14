namespace Fanex.Tool.TraceSP.Services
{
    using System.Configuration;
    static class AppSettingService
    {
        public static string DbListXMLPath
        {
            get
            {
                return ConfigurationManager.AppSettings["DbListXMLPath"].ToString();
            }
        }
    }
}
