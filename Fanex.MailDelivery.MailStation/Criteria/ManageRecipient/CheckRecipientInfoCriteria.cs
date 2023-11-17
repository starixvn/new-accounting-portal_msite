namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Check Recipient Information Criteria.
    /// </summary>
    public class CheckRecipientInfoCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientName")]
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_InsertRecipient";
        }

        public override bool IsValid() => true;
    }
}
