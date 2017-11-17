namespace Fanex.Maintenance.Service.Impl.LicenseeSetting
{
    using System.Collections.Generic;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.LrfDomain;
    using Fanex.Maintenance.Repository.Impl.LicenseeSetting;
    using Fanex.Maintenance.Service.Interface.LicenseeSetting;

    /// <summary>
    /// Licensee Service.
    /// </summary>
    public class LicenseeSettingService : ILicenseeSettingService
    {
        private readonly LicenseeSettingRepository _licenseeSettingRepository = new LicenseeSettingRepository();

        /// <summary>
        /// Get all domain.
        /// </summary>
        /// <returns>Last Domain.</returns>
        public IEnumerable<LrfDomain> GetAll()
        {
            var criteria = new GetAllLrfDomainCriteria();

            return _licenseeSettingRepository.Fetch(criteria);
        }

        /// <summary>
        /// Active Domain Up.
        /// </summary>
        /// <param name="id">Domain Identify.</param>
        public void ActiveDomain(int id)
        {
            var criteria = new ActiveDomainCriteria
            {
                Id = id
            };

            _licenseeSettingRepository.ActiveDomain(criteria);
        }
    }
}
