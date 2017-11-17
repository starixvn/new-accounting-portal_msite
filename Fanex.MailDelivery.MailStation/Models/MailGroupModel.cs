namespace Fanex.MailDelivery.MailStation.Models
{
    /// <summary>
    /// Mail Log Model.
    /// </summary>
    public class MailGroupModel
    {
        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public int MailRelevantCount { get; set; }
    }
}
