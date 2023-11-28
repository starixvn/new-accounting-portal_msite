using Fanex.Logging;
using Fanex.Tool.TraceSP.Criteria;
using Fanex.Tool.TraceSP.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Tool.TraceSP.Services
{
    using System.Linq;
    using Data.Repository;

    internal interface ITracingSpService
    {
        List<TracingSpViewModel> GetTracingSpData(TracingSp model);
    }

    internal class TracingSpService : ITracingSpService
    {
        private readonly IDynamicRepository dynamicRepository = new DynamicRepository();

        private bool IsExistedError(TracingSpCriteria criteria, TracingSp model)
        {
            dynamicRepository.Execute(criteria, model.Server);

            model.HasError = criteria.HasError;
            model.ErrorMsg = criteria.ErrorMsg;

            return model.HasError;
        }

        public List<TracingSpViewModel> GetTracingSpData(TracingSp model)
        {
            var suffixKey = model.Site.Equals("FPS", StringComparison.CurrentCultureIgnoreCase)
                ? "FPS"
                : string.Empty;

            var criteria = new TracingSpCriteria
            {
                Site = model.Site,
                Mode = model.Mode,
                TextDataLike = model.TextDataLike,
                TextDataLike2 = model.TextDataLike2,
                TextDataLike3 = model.TextDataLike3,
                TraceStartEvents = model.TraceStartEvents,
                TraceTimer = model.TraceTimer,
                ErrorMsg = string.Empty,
                HasError = false,
                SuffixKey = suffixKey
            };
            if (IsExistedError(criteria, model))
            {
                return new List<TracingSpViewModel>();
            }

            try
            {
                return dynamicRepository.Fetch<TracingSpViewModel>(criteria, model.Server).ToList();
            }
            catch (Exception e)
            {

                var message = "SOURCE: " + e.Source +
                              "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                              "\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                              "\r\nFORM: " + HttpContext.Current.Request.Form +
                              "\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString +
                              "\r\nTARGET-SITE: " + e.TargetSite +
                              "\r\nSTACKTRACE: " + e.StackTrace;

                Logger.Log.Warn(message);
                throw;
            }
        }
    }
}
