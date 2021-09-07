using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.DeleteTempVariable.Criteria;
using Fanex.DeleteTempVariable.Model;
using System.Linq;
using Fanex.DeleteTempVariable.Requests;

namespace Fanex.DeleteTempVariable.Services
{
    public interface ParlayLockingService
    {
        List<MatchParlayLocking> GetMatches(ParlayLockingRequest request);

        int DeleteMatch(DeleteParlayLockingRequest request);
    }

    public class ParlayLockingServiceImp : ParlayLockingService
    {
        private readonly IDynamicRepository repository;

        public ParlayLockingServiceImp()
        {
            repository = new DynamicRepository();
        }

        public List<MatchParlayLocking> GetMatches(ParlayLockingRequest request)
        {
            var criteria = new GetParlayLockingCriteria
            {
                MatchId = request.MatchId ?? 0,
                TimeRange = request.TimeRange
            };

            return repository.Fetch<MatchParlayLocking>(criteria).ToList();
        }

        public int DeleteMatch(DeleteParlayLockingRequest request)
        {
            int row = 0;
            var criteria = new DeleteParlayLockingCriteria
            {
                Session = request.Session
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