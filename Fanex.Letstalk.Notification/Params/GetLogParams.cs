using System;

namespace Fanex.Letstalk.Notification.Params
{
    public class GetLogParams
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string TargetName { get; set; }

        public string Group { get; set; }

        public int Id { get; set; }

        public byte Status { get; set; }
    }
}
