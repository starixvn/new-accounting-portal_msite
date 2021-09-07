using Fanex.Data.Repository;

namespace Fanex.DeleteTempVariable.Criteria
{
    public class UpdateSystemSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_Set_SystemSettingsUpd";
        }

        public override bool IsValid() => true;
    }
}