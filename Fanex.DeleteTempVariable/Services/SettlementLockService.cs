using System;
using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.DeleteTempVariable.Criteria;
using Fanex.DeleteTempVariable.Model;
using System.Linq;
using Fanex.DeleteTempVariable.Requests;

namespace Fanex.DeleteTempVariable.Services
{
    public interface SettlementLockService
    {
        List<MatchSettlementLock> GetMatches(DeleteTempRequest request);

        int Delete(DeleteSettlementProcessingRequest request);

        int UpdateSettlementLogs(UpdateSettlementLogRequest request);
    }

    public class SettlementLockServiceImp : SettlementLockService
    {
        private readonly IDynamicRepository repository;

        public SettlementLockServiceImp()
        {
            repository = new DynamicRepository();
        }

        public List<MatchSettlementLock> GetMatches(DeleteTempRequest request)
        {
           
            var criteria = new GetSettlementLockCriteria
            {
                MatchId = request.MatchId,
            };

            var dbtransData = repository.Fetch<MatchSettlementLock>(criteria).ToList();
            dbtransData.ForEach(m => m.DBServer = DataBaseServers.DBTrans);
           
            var replicatedData = GetDataReplicatedServer(request.MatchId);

            if (replicatedData.Any())
            {
                dbtransData.AddRange(replicatedData);
            }

            return dbtransData;
        }

        private List<MatchSettlementLock> GetDataReplicatedServer(int? matchId)
        {
            var replicatedData = new List<MatchSettlementLock>();
            var listReplicatedServer = new List<string> { "DB052", "DB053", "DB054", "DB055", "DB06", "DBTP" }; 
            foreach (var server in listReplicatedServer)
            {
                var replicatedServerCriteria = new GetSettlementLockReplicatedServerCriteria
                {
                    MatchId = matchId,
                    DbServer = server
                };

                var data = repository.Fetch<MatchSettlementLock>(replicatedServerCriteria).ToList();
                data.ForEach(m => m.DBServer = server);

                if (data.Any())
                {
                    replicatedData.AddRange(data);
                }
            }

            return replicatedData;
        }

        public int Delete(DeleteSettlementProcessingRequest request)
        {
            var row = 0;
            var criteria = new DeleteSettlementLockCriteria
            {
                SessionId = request.SessionId,
                DbServer = request.DbServer,
                IsSettleLock = request.isSettleLock
            };

            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                row = db.ExecuteNonQuery(hash);
            }

            return row;
        }

        public int UpdateSettlementLogs(UpdateSettlementLogRequest request)
        {
            var row = 0;
            var criteria = new UpdateSettlementLogCriteria
            {
                SessionId = request.SessionId,
                DbServer = request.DbServer,
                StartTime = request.StartTime
            };

            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                row = db.ExecuteNonQuery(hash);
            }

            return row;
        }
    }
}