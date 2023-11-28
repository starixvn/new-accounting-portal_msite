namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Data.Repository;

    public class GetAllMailGroupCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey() => "MailStation_GetAllMailGroup";
    }
}
