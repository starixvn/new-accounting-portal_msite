using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    public class GetMailGroupCriteria : CriteriaBase
    {
        public string MailGroupName { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_GetMailGroup";
        }
    }
}
