using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.DeleteTempVariable.Criteria;
using Fanex.DeleteTempVariable.Model;
using System.Linq;
using Fanex.DeleteTempVariable.Requests;

namespace Fanex.DeleteTempVariable.Services
{
    public interface MatchService
    {
        int TicketCount(MatchTicketRequest request);

        List<Match> GetMatches(int? matchId);

        Match GetMatchDetail(int matchId);

        int DeleteMatch(DeleteTempRequest request);
    }

    public class MatchServiceImp : MatchService
    {
        private readonly IDynamicRepository repository;

        public MatchServiceImp()
        {
            repository = new DynamicRepository();
        }

        public int TicketCount(MatchTicketRequest request)
        {
            var criteria = new GetTicketCriteria
            {
                MatchId = request.MatchId,
                KeyId = request.KeyId,
                Action = request.ActionName,
                MarketId = request.MarketId ?? string.Empty
            };

            var model = repository.Get<MatchTicket>(criteria);
            return model.TicketCount;
        }

        public List<Match> GetMatches(int? matchId)
        {
            var criteria = new GetMatchesCriteria
            {
                MatchId = matchId ?? 0
            };

            return repository.Fetch<Match>(criteria).ToList();
        }

        public Match GetMatchDetail(int matchId)
        {
            var criteria = new GetMatchesCriteria
            {
                MatchId = matchId
            };

            return repository.Get<Match>(criteria);
        }

        public int DeleteMatch(DeleteTempRequest request)
        {
            int row = 0;
            var criteria = new DeleteMatchCriteria
            {
                MatchId = request.MatchId,
                KeyId = request.KeyId,
                MarketId = request.MarketId
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