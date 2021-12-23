using System;

namespace EBroker.Management.Application.Shared
{
    public static class DateTimeProvider
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> NowSell = () => DateTime.Now;

        public static void SetDateTime(DateTime dateTimeNow)
        {
            Now = () => dateTimeNow;
        }

        public static void ResetDateTime()
        {
            Now = () => DateTime.Now;
        }
        public static void SetDateTimeSell(DateTime dateTimeNow)
        {
            NowSell = () => dateTimeNow;
        }

        public static void ResetDateTimeSell()
        {
            NowSell = () => DateTime.Now;
        }
    }
}
