namespace Fanex.Maintenance.Repository.Criteria.LrfDomain
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Update LRF Domain criteria.
    /// </summary>
    public class ActiveDomainCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "Lrf_ActiveDomainUpd";
        }

        public override bool IsValid()
        {
            return true;
        }  
    }
}
