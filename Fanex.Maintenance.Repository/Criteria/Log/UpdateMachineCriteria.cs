namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update machine criteria.
    /// </summary>
    public class UpdateMachineCriteria : CriteriaBase
    {
        public int MachineId { get; set; }

        public bool IsProduction { get; set; }

        public short Timezone { get; set; }

        public string MachineIP { get; set; }

        public override string GetSettingKey()
        {
            return "Logging_UpdateMachine";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}