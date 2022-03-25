using System;

namespace Fanex.Letstalk.Notification.Model
{
    public class NotificationModel
    {
        public int NotificationID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int RecipientID { get; set; }

        public string ConversationID { get; set; }

        public string RecipientName { get; set; }

        public int NumberAttachment { get; set; }

        public byte NotificationType { get; set; }

        public DateTime? ConfigSendTime { get; set; }

        public string Status { get; set; }

        public DateTime? LastUpdTime { get; set; }

        public DateTime? CreatedTime { get; set; }

        public int CreatedBy { get; set; }

        public string Creator { get; set; }

        public int LastModifiedBy { get; set; }

        public string LatestEditor { get; set; }

        public byte IsEdit { get; set; }

        public byte IsDelete { get; set; }
    }
}
