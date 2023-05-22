using Fanex.Data;
using Fanex.Logging;
using Fanex.Tool.TraceSP.Criteria;
using Fanex.Tool.TraceSP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Fanex.Tool.TraceSP.Services
{
    internal interface ITracingSpService
    {
        List<TracingSpViewModel> GetTracingSpData(ref TracingSp model);
    }

    internal class TracingSpService : ITracingSpService
    {
        private bool IsExistedError(IDbSetting dbSetting, Hashtable param, ref TracingSp model)
        {
            using (var objectDb = new ObjectDb(dbSetting))
            {
                objectDb.ExecuteNonQuery(param);
                model.HasError = Convert.ToBoolean(param["haserror"]);
                model.ErrorMsg = param["errormsg"].ToString();
            }

            return model.HasError;
        }

        public List<TracingSpViewModel> GetTracingSpData(ref TracingSp model)
        {
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
                HasError = false
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var dbSetting = db.DbSetting;
                var param = criteria.GetSpParams();
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(dbSetting.ConnectionString)
                {
                    DataSource = model.Server,
                    ConnectTimeout = 125
                };

                if (criteria.Site.Equals("FPS", StringComparison.CurrentCultureIgnoreCase))
                {
                    sqlConnectionStringBuilder.InitialCatalog = "perfmonDB";
                }

                dbSetting.ConnectionString = sqlConnectionStringBuilder.ToString();

                if (IsExistedError(dbSetting, param, ref model))
                {
                    return new List<TracingSpViewModel>();
                }

                using (var objectDb = new ObjectDb(dbSetting))
                {
                    try
                    {
                        var list = new List<TracingSpViewModel>();
                        using (var reader = objectDb.ExecuteReader(param))
                        {
                            while (reader.Read())
                            {
                                var viewModel = new TracingSpViewModel
                                {
                                    CPU = Convert.ToInt32(reader["CPU"].ToString()),
                                    DatabaseName = reader["DatabaseName"].ToString(),
                                    Duration = Convert.ToInt32(reader["Duration"].ToString()),
                                    EndTime = Convert.ToDateTime(reader["EndTime"].ToString()),
                                    Event = reader["Event"].ToString(),
                                    LoginName = reader["LoginName"].ToString(),
                                    Reads = Convert.ToInt32(reader["Reads"].ToString()),
                                    SPID = Convert.ToInt32(reader["SPID"].ToString()),
                                    StartTime = Convert.ToDateTime(reader["StartTime"].ToString()),
                                    TextData = reader["TextData"].ToString(),
                                    Writes = Convert.ToInt32(reader["Writes"].ToString())
                                };
                                list.Add(viewModel);
                            }
                        }

                        return list;
                    }
                    catch (Exception e)
                    {
                        Logger.Log.Error(e.Message);
                        throw;
                    }
                }
            }
        }
    }
}