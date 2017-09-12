using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data;
using Fanex.Tool.TraceSP.Criteria;
using Fanex.Tool.TraceSP.Models;

namespace Fanex.Tool.TraceSP.Services
{
    interface ITracingSpService
    {
        List<TracingSpViewModel> GetTracingSpData(TracingSp model);
    }

    class TracingSpService : ITracingSpService
    {
        public List<TracingSpViewModel> GetTracingSpData(TracingSp model)
        {
            var criteria = new TracingSpCriteria()
            {
                Site = model.Site,
                Mode = model.Mode,
                TextDataLike = model.TextDataLike,
                TextDataLike2 = model.TextDataLike2,
                TextDataLike3 = model.TextDataLike3,
                TraceStartEvents = model.TraceStartEvents,
                TraceTimer = model.TraceTimer
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var dbSetting = db.DbSetting;
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(dbSetting.ConnectionString);

                sqlConnectionStringBuilder.DataSource = model.Server;

                dbSetting.ConnectionString = sqlConnectionStringBuilder.ToString();

                using (var objectDb = new ObjectDb(dbSetting))
                {
                    return objectDb.Query<TracingSpViewModel>(criteria).ToList();
                }
            }
        }
    }
}
