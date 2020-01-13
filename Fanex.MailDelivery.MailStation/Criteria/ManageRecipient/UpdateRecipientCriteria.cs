using Fanex.Data.Repository;

namespace Fanex.MailDelivery.MailStation.Criteria.ManageRecipient
{
    public class UpdateRecipientCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "RecipientID")]
        public int RecipientId { get; set; }

        [SpParam(SqlParamName = "RecipientName")]
        public string Username { get; set; }

        public int EmailID1 { get; set; }

        public int EmailID2 { get; set; }

        public int EmailID3 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Note { get; set; }

        [SpParam(SqlParamName = "IsActive")]
        public bool Status { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_MailReport_UpdateRecipient";
        }
    }
}
