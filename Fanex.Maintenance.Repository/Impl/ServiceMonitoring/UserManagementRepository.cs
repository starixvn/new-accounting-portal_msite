// -----------------------------------------------------------------------
// <copyright file="UserManagementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Maintenance.Repository.Impl.ServiceMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Maintenance.Repository.Interface.ServiceMonitoring;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// User Management Repository.
    /// </summary>
    public class UserManagementRepository : IUserManagementRepository
    {
        /// <summary>
        /// Gets the list server.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>List Network login.</returns>
        public List<NetworkLogin> GetListServer(int userID)
        {
            var result = new List<NetworkLogin>();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamReader file = new StreamReader(File.Open(
                folderPath + "//ServerList.txt",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite)))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var elements = line.Split(',');
                        result.Add(new NetworkLogin()
                        {
                            IP = elements[0],
                            UserName = elements[1],
                            Password = elements[2]
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the list server.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="serverList">The server list.</param>
        public void UpdateListServer(int userID, List<NetworkLogin> serverList)
        {
            if (serverList != null && serverList.Count != 0)
            {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter file = new StreamWriter(File.Open(
                    folderPath + "//ServerList.txt",
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite)))
                {
                    foreach (var serverItem in serverList)
                    {
                        file.WriteLine(NetworkLoginToCSVString(serverItem));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list favorite services.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>List favorite services.</returns>
        public List<string> GetListFavouriteServices(int userID)
        {
            var result = new List<string>();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamReader file = new StreamReader(File.Open(
                folderPath + "//FavouriteServices.txt",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite)))
            {
                var line = file.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    var elements = line.Split(',');
                    result = new List<string>(line.Split(','));
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the list favorite services.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="servicesList">The services list.</param>
        public void UpdateListFavouriteServices(int userID, List<string> servicesList)
        {
            if (servicesList != null && servicesList.Count != 0)
            {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter file = new StreamWriter(File.Open(
                    folderPath + "//FavouriteServices.txt",
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite)))
                {
                    file.WriteLine(string.Join(",", servicesList.ToArray()));
                }
            }
        }

        /// <summary>
        /// Networks the login to CSV string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>CSV string.</returns>
        private string NetworkLoginToCSVString(NetworkLogin server)
        {
            return server.IP + ',' + server.UserName + ',' + server.Password;
        }
    }
}
