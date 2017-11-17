namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all machines criteria.
    /// </summary>
    public class GetAllMachinesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Logging_Machine_SelectAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}