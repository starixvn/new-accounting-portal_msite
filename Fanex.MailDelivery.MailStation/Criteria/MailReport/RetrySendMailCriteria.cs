namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.Data.Repository;

    public class RetrySendMailCriteria : CriteriaBase
    {
        public int TaskId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }

        public override bool IsValid() => true;
    }
}
