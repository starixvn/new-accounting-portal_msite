namespace Fanex.Maintenance.Repository.Criteria.ServiceMonitoring
{
    /// <summary>
    /// Service Monitoring Criteria.
    /// </summary>
    public class ServiceMonitoringCriteria
    {
        public string IP { get; set; }

        public string ServiceName { get; set; }

        public int Status { get; set; }

        public string ConfigFileName { get; set; }

        public string FileText { get; set; }
    }
}
