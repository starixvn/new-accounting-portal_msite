using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class GetGroupCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Notif_GetListRecipients";
        }
    }
}
