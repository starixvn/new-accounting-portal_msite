namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get log criteria.
    /// </summary>
    public class GetLogCriteria : CriteriaBase
    {
        public int? CategoryId { get; set; }

        public int? MachineId { get; set; }

        public string Severity { get; set; }

        public bool? IsProduction { get; set; }

        public bool? IsMonitored { get; set; }

        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public int TotalRows { get; set; }

        public int ToGMT { get; set; }

        public string SearchText { get; set; }

        public string RemovedLogCategories { get; set; }

        [SpParam(Ignored = true)]
        public bool GroupMessage { get; set; }

        [SpParam(Ignored = true)]
        public bool HasEdit { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Logging_Sel_Padding";
        }
    }
}