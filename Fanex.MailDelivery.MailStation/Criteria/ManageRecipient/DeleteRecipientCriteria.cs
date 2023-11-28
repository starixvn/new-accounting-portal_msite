using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Data;

    public class DeleteRecipientCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "MailStation_MailReport_DeleteRecipient";

        public override bool IsValid() => true;
    }
}
