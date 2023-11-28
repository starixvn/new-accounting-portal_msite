namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Data;
    using Data.Repository;

    public class MailLogCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "TaskID")]
        public int MailId { get; set; }

        public override string GetSettingKey() => "MailStation_MailReport_GetLogDetail";

        public override bool IsValid() => true;
    }
}
