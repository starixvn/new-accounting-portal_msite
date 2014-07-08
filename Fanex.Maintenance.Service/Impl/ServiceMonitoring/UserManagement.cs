// -----------------------------------------------------------------------
// <copyright file="UserManagement.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Maintenance.Service.Impl.ServiceMonitoring
{
    using System.Collections.Generic;
    using Fanex.Maintenance.Repository.Impl.ServiceMonitoring;
    using Fanex.Maintenance.Repository.Interface.ServiceMonitoring;
    using Fanex.Maintenance.Service.Interface.ServiceMonitoring;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserManagement : IUserManagement
    {
        private IUserManagementRepository _userManagement;

        public UserManagement()
        {
            this._userManagement = new UserManagementRepository();
        }

        public List<NetworkLogin> GetListServer(int userID)
        {
            return this._userManagement.GetListServer(userID);
        }

        public void UpdateListServer(int userID, List<NetworkLogin> serverList)
        {
            this._userManagement.UpdateListServer(userID, serverList);
        }

        public List<string> GetListFavouriteServices(int userID)
        {
            return this._userManagement.GetListFavouriteServices(userID);
        }

        public void UpdateListFavouriteServices(int userID, List<string> servicesList)
        {
            this._userManagement.UpdateListFavouriteServices(userID, servicesList);
        }
    }
}
