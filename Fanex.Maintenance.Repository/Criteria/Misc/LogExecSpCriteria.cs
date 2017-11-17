namespace Fanex.Maintenance.Repository.Criteria.Misc
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Execute SP log criteria.
    /// </summary>
    public class LogExecSpCriteria : CriteriaBase
    {
        public int CurrentUserId { get; set; }

        public DateTime LogDate { get; set; }

        public string LoginIp { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
