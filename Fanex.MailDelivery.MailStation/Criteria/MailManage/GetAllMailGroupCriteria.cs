namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetAllMailGroupCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllMailGroup";
        }
    }
}
