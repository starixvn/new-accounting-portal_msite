namespace Fanex.Maintenance.Site
{
    using System.Web.Mvc;
    using Caching;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;

    public static class OnebookUMConfig
    {
        public static void Register(GlobalFilterCollection filters)
        {
            var umConfig = new UMClientConfiguration().Initialize();
            var restClient = new UMRestClient(umConfig);
            var cacheService = new UMCacheService(new CacheService());
            var context = new HttpContextHelper();
            var maintenanceService = new MaintenanceService(restClient, cacheService, umConfig, context);

            var umFilter = new CheckUMFilterAttribute(maintenanceService, umConfig);

            UMClientManager
               .UseConfig(umConfig)
               .UseCacheService(cacheService)
               .UseRestClient(restClient)
               .UseHttpContext(context);

            filters.Add(umFilter);
        }
    }
}
