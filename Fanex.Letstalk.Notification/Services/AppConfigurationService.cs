using System.Configuration;

namespace Fanex.Letstalk.Notification.Services
{
    public interface IAppConfigurationService
    {
        string LetstalkApiUrl { get; }
    }

    public class AppConfigurationService : IAppConfigurationService
    {
        public string LetstalkApiUrl => ConfigurationManager.AppSettings["LetstalkApiUrl"];
    }
}
