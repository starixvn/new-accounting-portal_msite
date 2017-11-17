namespace Fanex.Maintenance.Repository.Criteria.Misc
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Result criteria.
    /// </summary>
    public class ResultCriteria : CriteriaBase
    {
        public ResultCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
