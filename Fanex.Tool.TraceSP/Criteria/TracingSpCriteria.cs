namespace Fanex.Tool.TraceSP.Criteria
{
    using Fanex.Data.Repository;
    public class TracingSpCriteria : CriteriaBase
    {
        public string Site { get; set; }

        public int TraceTimer { get; set; }

        public string TextDataLike { get; set; }

        public string TextDataLike2 { get; set; }

        public string TextDataLike3 { get; set; }

        public bool TraceStartEvents { get; set; }

        public bool Mode { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "vndba_sp_trace_WebApp";
        }
    }
}
