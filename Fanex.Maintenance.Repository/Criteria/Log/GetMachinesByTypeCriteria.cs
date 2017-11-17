namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get machine by type criteria.
    /// </summary>
    public class GetMachinesByTypeCriteria : CriteriaBase
    {
        public bool? MachineType { get; set; }

        public override string GetSettingKey()
        {
            return "Logging_Machine_ByType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}