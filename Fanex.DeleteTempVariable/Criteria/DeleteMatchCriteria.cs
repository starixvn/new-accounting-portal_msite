using Fanex.Data.Repository;

namespace Fanex.DeleteTempVariable.Criteria
{
    public class DeleteMatchCriteria : CriteriaBase
    {
        public int? MatchId { get; set; }

        public int KeyId { get; set; }

        public string MarketId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_MatchLockingDel";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}