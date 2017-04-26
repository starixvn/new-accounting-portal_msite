using System.Configuration;

namespace Fanex.Letstalk.Notification.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        public string LetstalkApiUrl => ConfigurationManager.AppSettings["LetstalkApiUrl"];
    }
}
