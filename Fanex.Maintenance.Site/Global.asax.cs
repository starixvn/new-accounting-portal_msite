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
    using ONEbook.UM.Filters;

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

            GlobalFilters.Filters.Add(new CheckUMFilter());

            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            LogProvider.SetCurrentLogProvider(new RabbitMQLogProvider());
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