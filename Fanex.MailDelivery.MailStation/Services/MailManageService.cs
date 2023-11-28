using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStation.Configurations;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.MailDelivery.MailStation.Models;

namespace Fanex.MailDelivery.MailStation.Services
{
    using Data.Repository;
    using Maintenance.Service.Command;

    public class MailManageService : MailStationBaseService
    {
        private IJobOperationService jobOperationService;

        private readonly IDynamicRepository dynamicRepository= new DynamicRepository();
      
        public IList<MailModel> MailList(GetMailListCriteria criteria)
        {
            var (mailModels, listdDates) = dynamicRepository.FetchMultiple<MailModel, CurrentTime>(criteria);
            var now = listdDates.FirstOrDefault().Now;

            mailModels.ToList().ForEach(n =>
            {
                if (n.TriggerType == (int)MailType.Schedule)
                {
                    n.IsClientOnline = n.LastActive != null && n.LastActive.Value.AddSeconds(AppSettings.ClientNotifyTimeOutInSecond) >= now;
                }
                else
                {
                    // Function and others always true
                    n.IsClientOnline = true;
                }
            });

            return mailModels.ToList();
        }

        public MailModel MailById(GetMailByIdCriteria criteria)
        {
            return dynamicRepository.Get<MailModel>(criteria);
        }

        public IList<MailActionLogModel> MailLogById(GetMailLogByIdCriteria criteria)
        {
            return dynamicRepository.Fetch<MailActionLogModel>(criteria).ToList();
        }

        public CommandResult CreateMail(CreateMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CommandResult UpdateMail(UpdateMailCriteria criteria)
        {
            var result = ExecuteNonQuery(criteria);

            if (result.IsSuccessful && criteria.TriggerType == (int)MailType.Schedule)
            {
                using (jobOperationService = new JobOperationService())
                {
                    jobOperationService.PublishScheduleChange(new Schedule
                    {
                        Id = criteria.MailId,
                        Active = criteria.IsActive,
                        EndTime = criteria.EndOn,
                        StartTime = criteria.StartOn ?? DateTime.Now,
                        MaxRetryTimes = criteria.Attempt,
                        Name = criteria.MailName,
                        RepeatExpression = criteria.CronExpression,
                        RetryInterval = criteria.Interval,
                        SiteId = criteria.SiteId
                    });
                }
            }

            return result;
        }

        public CommandResult DeleteMail(DeleteMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CommandResult CreateMailGroup(CreateMailGroupCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public void RunNow(int mailId)
        {
            var schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailId });

            using (jobOperationService = new JobOperationService())
            {
                jobOperationService.RunNow(schedule);
            }
        }
    }
}
