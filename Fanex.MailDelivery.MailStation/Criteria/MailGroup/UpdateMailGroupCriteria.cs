namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    public class UpdateMailGroupCriteria : UpdateCommandBase
    {
        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_UpdateMailGroup";
        }
    }
}
