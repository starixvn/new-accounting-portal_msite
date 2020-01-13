using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    public class DeleteRecipientCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_DeleteRecipient";
        }
    }
}