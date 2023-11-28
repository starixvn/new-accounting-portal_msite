using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    public class GetMailLogByIdCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override bool IsValid()
        {
            return MailId > 0;
        }

        public override string GetSettingKey()
        {
            return "Mail_MailManagement_GetLog";
        }
    }
}
