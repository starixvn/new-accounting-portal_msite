namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.Data.Repository;

    public class GetMailTaskByIdCriteria : CriteriaBase
    {
        public int MailTaskId { get; set; }

        public override string GetSettingKey()
        {
            return "Mail_Report_GetMailTaskByID";
        }
    }
}
