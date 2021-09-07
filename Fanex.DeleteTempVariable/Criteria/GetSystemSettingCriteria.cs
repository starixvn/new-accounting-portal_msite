using Fanex.Data.Repository;

namespace Fanex.DeleteTempVariable.Criteria
{
    public class GetSystemSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_Set_SystemSettingsSel";
        }

        public override bool IsValid() => true;
    }
}