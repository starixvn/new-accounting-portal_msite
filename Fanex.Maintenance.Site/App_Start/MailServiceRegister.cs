namespace Fanex.Maintenance.Site
{
    using System.Configuration;
    using Fanex.MailDeliver.MailClient;

    public static class MailServiceRegister
    {
        public static void Register()
        {
            AppSettings.AppMailId = ConfigurationManager.AppSettings["app_mail_id"];
            AppSettings.AppMailAddress = ConfigurationManager.AppSettings["app_mail_address"];
        }
    }
}
