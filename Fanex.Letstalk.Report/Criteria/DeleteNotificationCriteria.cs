using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class DeleteNotificationCriteria : CriteriaBase
    {
        public int NotificationID { get; set; }

        public int RecipientID { get; set; }

        public int LastModifiedBy { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "Notif_DeleteNotification";
    }
}
