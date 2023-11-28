using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Data;

    public class RecipientByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientById";
        }

        public override bool IsValid() => true;
    }
}
