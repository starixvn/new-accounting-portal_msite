using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Letstalk.Notification.Model
{
    public class NotificationRecipientModel
    {
        public int NotificationID { get; set; }

        public int RecipientID { get; set; }

        public string ConversationID { get; set; }

        public string RecipientName { get; set; }

        public string Status { get; set; }
    }
}
