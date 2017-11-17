namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Context;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Web.Auth;

    /// <summary>
    /// Base controller.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Reviewed.")]
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly CurrentContext CurrentContext;

        private readonly bool _enableDirectLogin = NapSettings.Settings.EnableDirectLogin;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// </summary>
        /// <value>0 : users must login through NAP Hub.</value>
        public bool EnableDirectLogin
        {
            get
            {
                return _enableDirectLogin;
            }
        }

        /// <summary>
        /// Creates authorize information.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <param name="onlineUserId">The online user ID.</param>
        protected void CreateAuthInfo(User user, int onlineUserId)
        {
            string currentDomainPath = CurrentContext.CurrentDomain.Path;

            var authInfo = new AuthInfo
            {
                UserId = user.Id,
                OnUserId = onlineUserId
            };

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomainPath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomainPath,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }
    }
}