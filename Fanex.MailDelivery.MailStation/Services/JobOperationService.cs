using EasyNetQ;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStation.Configurations;
using System;

namespace Fanex.MailDelivery.MailStation.Services
{
    public interface IJobOperationService : IDisposable
    {
        void PublishScheduleChange(Schedule schedule);

        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);
    }

    public class JobOperationService : IJobOperationService
    {
        private readonly IBus bus;

        public JobOperationService()
        {
            bus = RabbitHutch.CreateBus(
                    AppSettings.RabbitMQConnectionString,
                    serviceRegister =>
                    {
                        //serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                    });
        }

        public void PublishScheduleChange(Schedule schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, updateSchedulePublishId);
        }

        public void RunNow(Schedule schedule)
        {
            var runnowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, runnowJobPublishId);
        }

        public void Retry(Schedule schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, retryJobPublishId);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Free managed resources
                }

                bus?.Dispose();

                disposedValue = true;
            }
        }

        ~JobOperationService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}