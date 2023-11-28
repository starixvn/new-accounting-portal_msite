namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Data.Repository;

    public class GetMailTaskByIdCriteria : CriteriaBase
    {
        public int MailTaskId { get; set; }

        public override string GetSettingKey() => "Mail_Report_GetMailTaskByID";

        public override bool IsValid() => true;
    }
}
