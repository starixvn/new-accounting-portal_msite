namespace Fanex.MailDelivery.MailStation.Criteria
{
    using Data.Repository;

    public abstract class UpdateCommandBase : NonQueryCommand
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
