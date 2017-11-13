namespace Fanex.Tool.TraceSP.Models
{
    using System;
    using System.Web;
    using System.Xml;
    class TracingSpViewModel
    {
        private string _textData;
        public string Event { get; set; }

        public int SPID { get; set; }

        public string TextData
        {
            get
            {
                return HttpContext.Current.Server.HtmlEncode(_textData.Replace("><", "> <").Replace(",", ", "));
            }
            set
            {
                _textData = value;
            }
        }

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
