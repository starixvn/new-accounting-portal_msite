namespace Fanex.Letstalk.Notification.Services
{
    public interface IHttpClientService
    {
        T PostAsync<T, P>(string action, P inputParams);
    }
}
