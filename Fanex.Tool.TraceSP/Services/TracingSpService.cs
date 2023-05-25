using Fanex.Data;
using Fanex.Logging;
using Fanex.Tool.TraceSP.Criteria;
using Fanex.Tool.TraceSP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

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
                                    CPU = reader["CPU"].ToString(),
                                    DatabaseName = reader["DatabaseName"].ToString(),
                                    Duration = reader["Duration"].ToString(),
                                    EndTime = reader["EndTime"].ToString(),
                                    Event = reader["Event"].ToString(),
                                    LoginName = reader["LoginName"].ToString(),
                                    Reads = reader["Reads"].ToString(),
                                    SPID = reader["SPID"].ToString(),
                                    StartTime = reader["StartTime"].ToString(),
                                    TextData = reader["TextData"].ToString(),
                                    Writes = reader["Writes"].ToString()
                                };
                                list.Add(viewModel);
                            }
                        }

                        return list;
                    }
                    catch (Exception e)
                    {
                        var message = "SOURCE: " + e.Source +
                                      "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                                      "\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                                      "\r\nFORM: " + HttpContext.Current.Request.Form +
                                      "\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString +
                                      "\r\nTARGETSITE: " + e.TargetSite +
                                      "\r\nSTACKTRACE: " + e.StackTrace;

                        Logger.Log.Warn(message);
                        throw;
                    }
                }
            }
        }
    }
}