namespace Fanex.Maintenance.Repository.Criteria.LrfDomain
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All LRF Domain Criteria.
    /// </summary>
    public class GetAllLrfDomainCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Lrf_DomainSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
