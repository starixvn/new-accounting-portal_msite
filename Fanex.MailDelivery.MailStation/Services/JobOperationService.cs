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
        private readonly IBus legacyBus;

        public JobOperationService()
        {
            bus = RabbitHutch.CreateBus(AppSettings.RabbitMQConnectionString);
            legacyBus = RabbitHutch.CreateBus(
                AppSettings.RabbitMQConnectionString,
                serviceRegister => serviceRegister.Register<ITypeNameSerializer>(serviceProvider => new LegacyTypeNameSerializer()));
        }

        public void PublishScheduleChange(Schedule schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, updateSchedulePublishId);

            legacyBus.Publish(schedule, updateSchedulePublishId);
        }

        public void RunNow(Schedule schedule)
        {
            var runNowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, runNowJobPublishId);

            legacyBus.Publish(schedule, runNowJobPublishId);
        }

        public void Retry(Schedule schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, retryJobPublishId);

            legacyBus.Publish(schedule, retryJobPublishId);
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
                legacyBus?.Dispose();

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