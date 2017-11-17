namespace Fanex.Maintenance.Service.Interface.LicenseeSetting
{
    using System.Collections.Generic;
    using Fanex.Maintenance.Core.Model;

    /// <summary>
    /// Licensee Service Interface.
    /// </summary>
    public interface ILicenseeSettingService
    {
        /// <summary>
        /// Get all domain.
        /// </summary>
        /// <returns>List Domain.</returns>
        IEnumerable<LrfDomain> GetAll();

        /// <summary>
        /// Active Domain Up.
        /// </summary>
        /// <param name="id">Domain Identify.</param>
        void ActiveDomain(int id);
    }
}
