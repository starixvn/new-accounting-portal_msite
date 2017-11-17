namespace Fanex.Maintenance.Service.Impl.AppLog
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Helpers;
    using Fanex.Data.Repository;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.Log;

    /// <summary>
    /// Log category service.
    /// </summary>
    public class LogCategoryService
    {
        private readonly ReadonlyRepository<LogCategory> _categoryRepository = new ReadonlyRepository<LogCategory>();

        /// <summary>
        /// Create log category service.
        /// </summary>
        /// <returns>Log category service.</returns>
        public static LogCategoryService Create()
        {
            return new LogCategoryService();
        }

        /// <summary>
        /// Get all category.
        /// </summary>
        /// <returns>List of log category.</returns>
        public IEnumerable<LogCategory> GetAll()
        {
            var criteria = new GetAllCategoryCriteria();

            return _categoryRepository
                .Fetch(criteria)
                .Where(logCategory =>
                {
                    foreach (var categoryName in AlphaCategoryHelper.AlphaCategories)
                    {
                        if (logCategory.CategoryName.ToLower().Contains(categoryName))
                        {
                            return false;
                        }
                    }

                    return true;
                });
        }
    }
}