using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    using System.Runtime.CompilerServices;

    public class DeleteNotificationCriteria : NonQueryCommand
    {
        public int NotificationID { get; set; }

        public int LastModifiedBy { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => "Notif_DeleteNotification";
    }
}
