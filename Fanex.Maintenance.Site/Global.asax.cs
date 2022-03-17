namespace Fanex.Maintenance.Site
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using App_Start;
    using Data.Configuration;
    using EasyNetQ.Logging;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;
    using MailDelivery.MailStation.Logging;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
            FanexLogging.Register();
            MailServiceRegister.Register();

            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());

            OnebookUMConfig.Register(GlobalFilters.Filters);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!Request.IsLocal)
            {
                ExceptionHandler.Handle(Server.GetLastError());
            }
        }
    }
}