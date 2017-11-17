namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert criteria.
    /// </summary>
    public class InsertMachineCriteria : CriteriaBase
    {
        public string MachineName { get; set; }

        public bool IsProduction { get; set; }

        public short Timezone { get; set; }

        public string MachineIP { get; set; }

        public override string GetSettingKey()
        {
            return "Logging_InsertMachine";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}