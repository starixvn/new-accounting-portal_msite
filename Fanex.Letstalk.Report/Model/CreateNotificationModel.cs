using System;

namespace Fanex.Letstalk.Notification.Model
{
    public class CreateNotificationModel
    {
        public string LetstalkGroup { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsSendNow { get; set; }

        public DateTime SendTime { get; set; }
    }
}
