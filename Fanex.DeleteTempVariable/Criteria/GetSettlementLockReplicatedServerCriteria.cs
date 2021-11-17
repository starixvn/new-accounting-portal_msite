using Fanex.Data.Repository;

namespace Fanex.DeleteTempVariable.Criteria
{
    public class GetSettlementLockReplicatedServerCriteria : CriteriaBase
    {
        public int? MatchId { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public override string GetSettingKey()
        {
            return $"NAP_Set_MatchLockingReplicatedServerSel_{DbServer}";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}