namespace Fanex.Maintenance.Repository.Criteria.Log
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetAllCategoryCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Logging_Category_SelAll";
        }
    }
}
