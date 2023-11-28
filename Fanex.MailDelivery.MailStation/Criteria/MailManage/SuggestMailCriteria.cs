namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Data;
    using Fanex.Data.Repository;

    public class SuggestMailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "EmailAddress")]
        public string Query { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllSuggestedMails";
        }

        public override bool IsValid() => true;
    }
}
