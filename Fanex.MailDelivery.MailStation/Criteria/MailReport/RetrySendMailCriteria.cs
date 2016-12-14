namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Retry Send Mail Criteria.
    /// </summary>
    public class RetrySendMailCriteria : CriteriaBase
    {
        public int TaskId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }
    }
}
