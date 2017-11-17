namespace Fanex.MailDelivery.MailStation.Services
{
    using Fanex.NAP.Core.Models;

    /// <summary>
    /// Application Session.
    /// </summary>
    public static class AppSession
    {
        public const string CurrentLogInUser = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentLogInUser";

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
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