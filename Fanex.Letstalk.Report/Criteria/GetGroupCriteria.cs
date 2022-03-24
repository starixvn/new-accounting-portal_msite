using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class GetGroupCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "Notif_GetListRecipients";
    }
}
