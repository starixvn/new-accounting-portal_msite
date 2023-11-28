using Fanex.MailDelivery.MailStation.Models;

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    public class UpdateMailCriteria : CreateMailCriteria
    {
        public UpdateMailCriteria()
            : base()
        {
        }

        public UpdateMailCriteria(MailModel model)
            : base(model)
        {
            if (model != null)
            {
                this.MailId = model.MailId;
            }
        }

        public int MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_EditMail";
        }
    }
}
