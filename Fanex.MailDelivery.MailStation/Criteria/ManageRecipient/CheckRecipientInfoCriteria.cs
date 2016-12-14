namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
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
    }
}
