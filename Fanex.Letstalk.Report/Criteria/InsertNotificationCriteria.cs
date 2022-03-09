using Fanex.Data.Repository;
using System;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class InsertNotificationCriteria : CriteriaBase
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string LstRecipientID { get; set; }

        public DateTime ConfigSendTime { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Notif_InsertNewNotification";
        }
    }
}
