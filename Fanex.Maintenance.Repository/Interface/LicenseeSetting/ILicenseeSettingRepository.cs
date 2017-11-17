namespace Fanex.Maintenance.Repository.Interface.LicenseeSetting
{
    using Fanex.Maintenance.Repository.Criteria.LrfDomain;

    /// <summary>
    /// Licensee Setting Repository Interface.
    /// </summary>
    public interface ILicenseeSettingRepository
    {
        /// <summary>
        /// Active Domain Up.
        /// </summary>
        /// <param name="criteria">Domain criteria.</param>
        void ActiveDomain(ActiveDomainCriteria criteria);
    }
}
