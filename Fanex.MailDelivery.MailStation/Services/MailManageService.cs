using Fanex.Data;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.MailStation.Configurations;
using Fanex.MailDelivery.MailStation.Criteria.MailGroup;
using Fanex.MailDelivery.MailStation.Criteria.MailManage;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.NAP.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.MailDelivery.MailStation.Services
{
    public class MailManageService : MailStationBaseService
    {
        private IJobOperationService jobOperationService;

        public MailManageService()
        {
        }

        public IList<MailModel> MailList(GetMailListCriteria criteria)
        {
            List<MailModel> mailModels;
            DateTime now;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                using (var multi = db.QueryMultiple())
                {
                    mailModels = multi.Read<MailModel>().ToList();
                    now = multi.Read<DateTime>().FirstOrDefault();
                }
            }

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

            return mailModels;
        }

        public MailModel MailById(GetMailByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());

                return db.Query<MailModel>().FirstOrDefault();
            }
        }

        public CommandResult CreateMail(CreateMailCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public CommandResult UpdateMail(UpdateMailCriteria criteria)
        {
            CommandResult result = ExecuteNonQuery(criteria);

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
            Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailId });

            using (jobOperationService = new JobOperationService())
            {
                jobOperationService.RunNow(schedule);
            }
        }
    }
}
