using Fanex.MailDeliver.MailClient;
using System.Configuration;

namespace Fanex.Maintenance.Site.App_Start
{
    public static class MailServiceRegister
    {
        public static void Register()
        {
            AppSettings.AppMailId = ConfigurationManager.AppSettings["app_mail_id"];
            AppSettings.AppMailAddress = ConfigurationManager.AppSettings["app_mail_address"];
        }
    }
}