namespace Fanex.MailDelivery.MailStation.Criteria.MailReport
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Mail Report Criteria.
    /// </summary>
    public class MailReportCriteria : CriteriaBase
    {
        public string MailName { get; set; }

        public int SiteId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_GetMailTaskList";
        }
    }
}
