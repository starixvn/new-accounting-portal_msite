using Fanex.Data.Repository;

namespace Fanex.Maintenance.Repository.Criteria.MailService
{
    public class GetMailConfigLogsCriteria : CriteriaBase
    {
        public int AppID { get; set; }
        public override bool IsValid() => true;
        public override string GetSettingKey() => "MailRecorder_SelLogConfig";
    }
}
