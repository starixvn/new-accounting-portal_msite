namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using Fanex.ConnectionString.Domain;

    /// <summary>
    /// The view model for search site.
    /// </summary>
    public class SearchSiteViewModel
    {
        private string _name;

        public SearchSiteViewModel()
        {
            _name = string.Empty;
            HasEditPermission = false;
        }

        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        public List<Site> Sites { get; set; }

        public bool HasEditPermission { get; set; }
    }
}
