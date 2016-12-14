namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Deletes Recipient Criteria.
    /// </summary>
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