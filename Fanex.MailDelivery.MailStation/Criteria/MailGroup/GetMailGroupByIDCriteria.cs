using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    public class GetMailGroupByIdCriteria : CriteriaBase
    {
        public int MailGroupId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_GetMailGroupByID";
        }
    }
}
