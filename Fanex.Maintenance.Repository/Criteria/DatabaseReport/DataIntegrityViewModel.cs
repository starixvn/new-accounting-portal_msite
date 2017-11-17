namespace Fanex.Maintenance.Repository.Criteria.DatabaseReport
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The data integrity view model.
    /// </summary>
    public class DataIntegrityViewModel
    {
        public int DrawFilter { get; set; }

        public string Date { get; set; }

        public DateTime GetDate()
        {
            return DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
