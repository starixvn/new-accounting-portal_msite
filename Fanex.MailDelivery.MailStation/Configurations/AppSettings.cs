using System;
using System.Web.Configuration;

namespace Fanex.MailDelivery.MailStation.Configurations
{
    public static class AppSettings
    {
        public static string RabbitMQConnectionString => WebConfigurationManager.AppSettings.Get("MailStation:RabbitMQConnectionString");

        public static int ClientNotifyTimeOutInSecond => Convert.ToInt32(WebConfigurationManager.AppSettings.Get("MailStation:ClientNotifyTimeOutInSecond"));
    }
}