namespace Fanex.Letstalk.Report.Services
{
    public interface IHttpClientService
    {
        T PostAsync<T, P>(string action, P inputPrams);
    }
}
