namespace Fanex.MailDelivery.MailStation.Services
{
    using Fanex.NAP.Core.Models;

    public static class AppSession
    {
        public const string CurrentLogInUser = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentLogInUser";
               
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(CurrentLogInUser, value);
            }
        }
    }
}
