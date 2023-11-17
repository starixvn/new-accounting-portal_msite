namespace Fanex.Maintenance.Repository.Criteria.Misc
{
    /// <summary>
    /// Result criteria.
    /// </summary>
    public class ResultCriteria
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
