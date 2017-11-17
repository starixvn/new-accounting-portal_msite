namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class DeleteMailCriteria : CriteriaBase
    {
        public int MailId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_DeleteMail";
        }
    }
}
