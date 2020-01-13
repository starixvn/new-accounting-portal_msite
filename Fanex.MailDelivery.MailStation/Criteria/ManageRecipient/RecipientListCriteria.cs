using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    public class RecipientListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string UserName { get; set; }

        [SpParam(SqlParamName = "Email")]
        public string Emails { get; set; }

        public string Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientList";
        }
    }
}
