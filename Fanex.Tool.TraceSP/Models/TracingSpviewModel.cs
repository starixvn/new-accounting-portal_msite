namespace Fanex.Tool.TraceSP.Models
{
    using System;
    class TracingSpViewModel
    {
        public string Event { get; set; }

        public int SPID { get; set; }
        
        public string TextData { get; set; }

        public string LoginName { get; set; }

        public int Duration { get; set; }

        public int Reads { get; set; }

        public int Writes { get; set; }

        public int CPU { get; set; }

        public string DatabaseName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
