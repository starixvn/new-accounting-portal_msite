namespace Fanex.Maintenance.Service.Interface.Testing
{
    using Core.Model.LetstalkNotification;

    public interface INotificationService
    {
        MessageModel Query(QueryModel model);
    }
}