using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class CancelNotificationCriteria : CriteriaBase
    {
        public string LstNotificationID { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Notif_CancelNotification";
        }
    }
}
