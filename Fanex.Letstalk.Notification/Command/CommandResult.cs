namespace Fanex.Letstalk.Notification.Command
{
    public interface ICommandResult
    {
        int Code { get; }

        string Message { get; }

        bool IsSuccessful { get; }
    }


    public class CommandResult : ICommandResult
    {
        public bool IsSuccessful => Code == 0;

        public CommandResult()
            : this(0, string.Empty)
        {
        }

        public CommandResult(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }
    }
}
