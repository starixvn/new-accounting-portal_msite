using EasyNetQ;
using EasyNetQ.Topology;
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
        private readonly IAdvancedBus advancedBus;
        private readonly IExchange oldScheduleExchange;
        private const string OldScheduleExchangeName = "Fanex.MailDelivery.Core.Models.Schedule:Fanex.MailDelivery.Core";

        public JobOperationService()
        {
            bus = RabbitHutch.CreateBus(AppSettings.RabbitMQConnectionString);
            advancedBus = bus.Advanced;
            oldScheduleExchange = advancedBus.ExchangeDeclare(OldScheduleExchangeName, ExchangeType.Topic);
        }

        public void PublishScheduleChange(Schedule schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, updateSchedulePublishId);

            advancedBus.Publish(oldScheduleExchange, updateSchedulePublishId, true, new Message<Schedule>(schedule));
        }

        public void RunNow(Schedule schedule)
        {
            var runnowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, runnowJobPublishId);

            advancedBus.Publish(oldScheduleExchange, runnowJobPublishId, true, new Message<Schedule>(schedule));
        }

        public void Retry(Schedule schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, retryJobPublishId);

            advancedBus.Publish(oldScheduleExchange, retryJobPublishId, true, new Message<Schedule>(schedule));
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