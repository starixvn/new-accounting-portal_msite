namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get machine by ID criteria.
    /// </summary>
    public class GetMachineByIdCriteria : CriteriaBase
    {
        public int MachineId { get; set; }

        public override string GetSettingKey()
        {
            return "Logging_Machine_ById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}