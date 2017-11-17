namespace Fanex.MailDelivery.MailStation.Extensions
{
    using System;
    using System.Linq;
    using Fanex.MailDelivery.MailStation.Constants;

    /// <summary>
    /// Time Zone Extension.
    /// </summary>
    public static class TimeZoneExtension
    {
        public static string GetTimeZoneString()
        {
            TimeZoneInfo.ClearCachedData();
            string standardName = TimeZone.CurrentTimeZone.StandardName;
            var timezone = TimeZoneConstants.Timezones.FirstOrDefault(timeZone => timeZone.StandardName.Equals(standardName));
            return timezone == null ? null : timezone.TimeZoneString;
        }
    }
}
