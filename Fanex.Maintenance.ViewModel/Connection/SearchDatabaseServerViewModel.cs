namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using Fanex.ConnectionString.Domain;

    /// <summary>
    /// The search database server view model.
    /// </summary>
    public class SearchDatabaseServerViewModel
    {
        private string _name;

        public SearchDatabaseServerViewModel()
        {
            _name = string.Empty;
            HasEditPermission = false;
        }

        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        public bool IsActive { get; set; }

        public List<DatabaseServer> DatabaseServers { get; set; }

        public bool HasEditPermission { get; set; }
    }
}
