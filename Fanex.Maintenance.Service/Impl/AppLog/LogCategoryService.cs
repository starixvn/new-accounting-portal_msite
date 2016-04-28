// --------------------------------------------------------------------------
// <copyright file="LogCategoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
// </history>

namespace Fanex.Maintenance.Service.Impl.AppLog
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.Log;

    /// <summary>
    /// Log category service.
    /// </summary>
    public class LogCategoryService
    {
        private static IEnumerable<string> _removedLogCategories;
        private readonly ReadonlyRepository<LogCategory> _categoryRepository = new ReadonlyRepository<LogCategory>();

        private LogCategoryService()
        {
            if (_removedLogCategories == null)
            {
                var removedLogCategories = ConfigurationManager.AppSettings["RemovedLogCategories"];
                if (!string.IsNullOrEmpty(removedLogCategories))
                {
                    _removedLogCategories = removedLogCategories.ToLower().Split(',');
                }
            }
        }

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
                    foreach (var categoryName in _removedLogCategories)
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