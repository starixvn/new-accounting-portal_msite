namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Suggest Mail Criteria.
    /// </summary>
    public class SuggestMailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "EmailAddress")]
        public string Query { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllSuggestedMails";
        }
    }
}
