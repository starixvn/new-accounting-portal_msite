namespace Fanex.MailDelivery.MailStation.Extensions
{
    using Fanex.MailDelivery.MailStation.Constants;
    using System;
    using System.Linq;

    public static class TimeZoneExtension
    {
        public static string GetTimeZoneString()
        {
            TimeZoneInfo.ClearCachedData();
            var standardName = TimeZone.CurrentTimeZone.StandardName;
            var timezone = TimeZoneConstants.Timezones.FirstOrDefault(timeZone => timeZone.StandardName.Equals(standardName));
            return timezone == null ? null : timezone.TimeZoneString;
        }
    }
}
