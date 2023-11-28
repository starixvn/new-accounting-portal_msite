using Fanex.Data.Repository;

namespace Fanex.Letstalk.Notification.Criteria
{
    public class InsertAttachmentFileCriteria : CriteriaBase
    {
        public int NotificationID { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Notif_InsertNewAttachment";
        }
    }
}
