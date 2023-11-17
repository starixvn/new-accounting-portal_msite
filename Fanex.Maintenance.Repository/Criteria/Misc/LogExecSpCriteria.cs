namespace Fanex.Maintenance.Repository.Criteria.Misc
{
    using System;

    /// <summary>
    /// Execute SP log criteria.
    /// </summary>
    public class LogExecSpCriteria 
    {
        public int CurrentUserId { get; set; }

        public DateTime LogDate { get; set; }

        public string LoginIp { get; set; }
    }
}
