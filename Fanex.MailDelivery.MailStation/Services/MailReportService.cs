using Fanex.Common;
using Fanex.Data;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStation.Criteria;
using Fanex.MailDelivery.MailStation.Criteria.MailReport;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.Maintenance.Service.Impl.Mail;
using Fanex.Maintenance.Service.Interface.Mail;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.MailDelivery.MailStation.Services
{
    public class MailReportService : MailStationBaseService
    {
        private readonly IMailService mailService;
        private IJobOperationService jobOperationService;

        public MailReportService()
        {
            mailService = mailService ?? MailService.Create();
        }

        public List<MailTaskModel> MailReport(MailReportCriteria criteria)
        {
            int oldStatus = criteria.Status;
            criteria.Status = -1;
            List<MailTaskModel> mailTasks = GetMailTasks(criteria);

            if (criteria.TriggerType != (int)MailType.Function)
            {
                criteria.Status = oldStatus;
                GetAllScheduleCriteria getAllScheduleCriteria = new GetAllScheduleCriteria { MailName = criteria.MailName, SiteId = criteria.SiteId };
                List<MailTaskDetailModel> mailTaskDetails = GetMailTaskDetails(getAllScheduleCriteria);

                if (criteria.Status == -1 || criteria.Status == (int)JobStatus.Waiting || criteria.Status == (int)JobStatus.Missing)
                {
                    TimeSpan ts = new TimeSpan(23, 59, 59);
                    criteria.ToDate += ts;

                    foreach (MailTaskDetailModel schedule in mailTaskDetails)
                    {
                        DateTime fromDate = MaxDateTime(criteria.FromDate, schedule.StartOn).ToUniversalTime();
                        DateTime toDate = MinDateTime(criteria.ToDate, schedule.EndOn).ToUniversalTime();

                        if (fromDate <= toDate)
                        {
                            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(schedule.CronExpression).Build();
                            var listtriggerdate = TriggerUtils.ComputeFireTimesBetween(
                                (IOperableTrigger)trigger,
                                null,
                                new DateTimeOffset(fromDate),
                                new DateTimeOffset(toDate));
                            listtriggerdate = (from trigertime in listtriggerdate
                                               where !(from task in mailTasks
                                                       where task.MailId == schedule.MailId
                                                       select task.ScheduledTime)
                                                   .Contains(trigertime.LocalDateTime)
                                               select trigertime).ToList();

                            foreach (var triggertime in listtriggerdate)
                            {
                                var status = triggertime.LocalDateTime.AddSeconds(5) > DateTime.Now ? (int)JobStatus.Waiting : (int)JobStatus.Missing;

                                if (criteria.Status == -1 || criteria.Status == status)
                                {
                                    MailTaskModel waitingTask = new MailTaskModel
                                    {
                                        MailName = schedule.MailName,
                                        MailId = schedule.MailId,
                                        Attempt = schedule.Attempt,
                                        LastSent = null,
                                        ScheduledTime = triggertime.LocalDateTime,
                                        SiteId = schedule.SiteId,
                                        SiteName = schedule.SiteName,
                                        Status = status,
                                        TriggerType = (int)MailType.Schedule
                                    };

                                    mailTasks.Add(waitingTask);
                                }
                            }
                        }
                    }
                }
            }

            if (oldStatus != -1)
            {
                mailTasks = mailTasks.Where(m => m.Status == oldStatus).ToList();
            }

            return mailTasks.OrderBy(m => m.MailName).ThenByDescending(m => m.ScheduledTime).ThenByDescending(m => m.LastSent).ToList();
        }

        public MailTaskStatusCounterModel GetMailTaskStatusCounter()
        {
            var criteria = new MailReportCriteria
            {
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.Date,
                MailName = null,
                SiteId = -1,
                Status = -1,
                TriggerType = -1
            };

            List<MailTaskModel> mailTasks = MailReport(criteria);

            var counter = new MailTaskStatusCounterModel
            {
                MissingItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Missing),
                FailedItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.JobFailed),
                RunningItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Running),
                SentItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.Success),
                SucceededItemCount = mailTasks.Count(x => x.Status == (int)JobStatus.SuccessWithoutSendmail),
                CurrentDate = DateTime.Now.Date
            };

            return counter;
        }

        private static List<MailTaskDetailModel> GetMailTaskDetails(GetAllScheduleCriteria getAllScheduleCriteria)
        {
            using (ObjectDb db = new ObjectDb(getAllScheduleCriteria.GetSettingKey()))
            {
                db.AssignParamValues(getAllScheduleCriteria.GetSpParams());
                List<MailTaskDetailModel> mailTaskDetails = db.Query<MailTaskDetailModel>().ToList();

                return mailTaskDetails;
            }
        }

        private static List<MailTaskModel> GetMailTasks(MailReportCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());

                List<MailTaskModel> mailTasks = db.Query<MailTaskModel>().ToList();

                return mailTasks;
            }
        }

        public MailRecord MailDetail(MailDetailCriteria criteria)
        {
            try
            {
                return mailService.GetMail(criteria.MailId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        public List<MailLogModel> MailLog(MailLogCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<MailLogModel>().ToList();
            }
        }

        public void RetrySendMail(RetrySendMailCriteria criteria)
        {
            MailTaskModel mailTask = GetMailTaskByID(new GetMailTaskByIdCriteria { MailTaskId = criteria.TaskId });
            Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailTask.MailId });
            schedule.TaskId = mailTask.MailTaskId;

            UpdateRunningTask(mailTask.MailTaskId);

            using (jobOperationService = new JobOperationService())
            {
                jobOperationService.Retry(schedule);
            }
        }

        public void AbortTask(int taskId)
        {
            var criteria = new UpdateMailTaskStatusCriteria
            {
                TaskId = taskId,
                Status = (int)JobStatus.Abort,
                LastSent = DateTime.Now
            };

            ExecuteNonQuery(criteria);
        }

        public void AddMailLog(AddMailLogCriteria criteria)
        {
            ExecuteNonQuery(criteria);
        }

        public void RetryMissingJob(RetryMissingTaskCriteria criteria)
        {
            // Add Missing Task to DB first.
            int mailTaskId;
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                mailTaskId = Converter.ToInt(db.GetParameterValue("MailTaskID"));
            }

            MailTaskModel mailTask = GetMailTaskByID(new GetMailTaskByIdCriteria { MailTaskId = mailTaskId });
            Schedule schedule = GetScheduleByMailId(new GetScheduleByIdCriteria { MailId = mailTask.MailId });
            schedule.CurrentFireTime = criteria.ScheduledTime;
            schedule.TaskId = mailTaskId;

            using (jobOperationService = new JobOperationService())
            {
                jobOperationService.Retry(schedule);
            }
        }

        private void UpdateRunningTask(int taskId)
        {
            var criteria = new UpdateMailTaskStatusCriteria
            {
                TaskId = taskId,
                Status = (int)JobStatus.Running,
                LastSent = DateTime.Now
            };

            ExecuteNonQuery(criteria);
        }

        private DateTime MaxDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                // Add 5 seconds for the number ratio.
                return date1 >= date2.Value ? date1 : date2.Value.AddSeconds(5);
            }
            else
            {
                return date1;
            }
        }

        private DateTime MinDateTime(DateTime date1, DateTime? date2)
        {
            if (date2.HasValue)
            {
                // Add 5 seconds for the number ratio.
                return date1 < date2.Value ? date1 : date2.Value.AddSeconds(5);
            }
            else
            {
                return date1;
            }
        }
    }
}
