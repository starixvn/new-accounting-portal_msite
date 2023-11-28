using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Web.Session;

namespace Fanex.MailDelivery.MailStation.Services
{
    public static class PermissionService
    {
        private static readonly ICurrentSession currentSession = new CurrentSession();
        private static readonly IUserService userService = new UserService();

        public static bool HasReadPermission(string functionName)
        {
            return HasPermission(functionName);
        }

        public static bool HasHostPermission(string functionName)
        {
            return HasPermission(functionName, Constant.Policy.Host);
        }

        public static bool HasPermission(string functionName, Constant.Policy functionPolicy = Constant.Policy.Read)
        {
            return userService.IsHavingPermission(
                currentSession.UserId,
                NapSettings.Settings.SiteId,
                functionName,
                functionPolicy);
        }
    }
}
