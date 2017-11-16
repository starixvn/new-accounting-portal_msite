namespace Fanex.Maintenance.Site
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using App_Start;
    using Data.Configuration;
    using Logging.Extensions.Database;
    using Logging.Extensions.Web;
    using ONEbook.UM.Filters;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();

            GlobalFilters.Filters.Add(new CheckUMFilter());

            ExceptionHandler.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ExceptionHandler.Handle(Server.GetLastError());
        }
    }
}