namespace Fanex.Maintenance.Repository.Command
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

        private int _code;
        private string _message;

        public CommandResult()
            : this(0, string.Empty)
        {
        }

        public CommandResult(int code, string message)
        {
            this._code = code;
            this._message = message;
        }

        public int Code
        {
            get { return this._code; }
            set { this._code = value; }
        }

        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
    }
}
