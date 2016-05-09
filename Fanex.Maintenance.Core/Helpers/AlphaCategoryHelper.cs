namespace Fanex.Maintenance.Core.Helpers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AlphaCategoryHelper
    {
        private static IEnumerable<string> _alphaCategories;

        static AlphaCategoryHelper()
        {
            if (_alphaCategories == null)
            {
                var removedLogCategories = ConfigurationManager.AppSettings["RemovedLogCategories"];
                if (!string.IsNullOrEmpty(removedLogCategories))
                {
                    _alphaCategories = removedLogCategories.ToLower().Split(',');
                }
                else
                {
                    _alphaCategories = Enumerable.Empty<string>();
                }
            }
        }

        public static IEnumerable<string> AlphaCategories
        {
            get
            {
                return _alphaCategories;
            }
        }
    }
}