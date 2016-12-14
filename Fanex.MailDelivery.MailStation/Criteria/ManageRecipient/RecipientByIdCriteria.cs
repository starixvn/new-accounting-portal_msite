namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Recipient By Identify Criteria.
    /// </summary>
    public class RecipientByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetRecipientById";
        }
    }
}
