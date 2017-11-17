namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetMailGroupByIdCriteria : CriteriaBase
    {
        public int MailGroupId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_GetMailGroupByID";
        }
    }
}
