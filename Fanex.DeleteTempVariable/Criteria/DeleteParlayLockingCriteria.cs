using Fanex.Data.Repository;

namespace Fanex.DeleteTempVariable.Criteria
{
    public class DeleteParlayLockingCriteria : CriteriaBase
    {
        public string Session { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Set_ParlayLockingDel";
        }

        public override bool IsValid() => true;
    }
}