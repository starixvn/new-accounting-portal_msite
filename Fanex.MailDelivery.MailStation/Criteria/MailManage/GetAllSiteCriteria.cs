using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    public class GetAllSiteCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllSites";
        }
    }
}
