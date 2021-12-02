namespace Fanex.Maintenance.Site.App_Start
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;

    public static class FanexLogging
    {
        public static void Register()
        {
            StartLogSetting();
            var configuration = new LoggingConfiguration()
                .GetHttpHeaderInfo()
                .GetHttpSessionInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
            ExceptionHandler.UseConfig(configuration);
        }

        public static void HandleApplicationError(HttpServerUtility server)
        {
            var isAuthenticated = HttpContext.Current?.Request.IsAuthenticated;
            var customData = new Dictionary<string, object> { { "IsAuthenticated", isAuthenticated } };
            ExceptionHandler.Handle(
                server.GetLastError(),
                customData
            );
        }

        private static void StartLogSetting()
        {
            var logEngine = new List<ILogEngine>();
            var appSettings = ConfigurationManager.AppSettings;
            var sentryEngineOptions = new SentryEngineOptions
            {
                Dsn = new Dsn(appSettings["SentryToken"])
            };

            logEngine.Add(new SentryLogging(sentryEngineOptions));

            LogManager.SetDefaultLogCategory(appSettings["FanexLogCategory"]).Use(logEngine);
        }
    }
}