namespace Fanex.Maintenance.Core.Configurations
{
    using System.Configuration;

    public static class AppSettings
    {
        public static string RemovedLogCategories
            => ConfigurationManager.AppSettings["RemovedLogCategories"] ?? string.Empty;
    }
}