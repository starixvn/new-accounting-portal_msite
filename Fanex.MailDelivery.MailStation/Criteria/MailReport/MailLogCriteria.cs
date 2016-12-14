namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Mail Log Criteria.
    /// </summary>
    public class MailLogCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "TaskID")]
        public int MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetLogDetail";
        }
    }
}
