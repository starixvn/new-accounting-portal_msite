namespace Fanex.Maintenance.Service.Criteria.MailService
{
    using Data.Repository;

    public class GetMailConfigLogsCriteria : CriteriaBase
    {
        public int AppID { get; set; }
        public override bool IsValid() => true;
        public override string GetSettingKey() => "MailRecorder_SelLogConfig";
    }
}
