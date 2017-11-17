namespace Fanex.ServiceManagement.Model
{
    public class NetworkLogin
    {
        public NetworkLogin()
        {
            this.IP = string.Empty;
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }

        public NetworkLogin(string ip, string username, string password)
        {
            this.IP = ip;
            this.UserName = username;
            this.Password = password;
        }

        public string IP { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
