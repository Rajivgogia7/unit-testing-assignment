using System;

namespace EBroker.Management.Application.Shared
{
    public static class DateTimeProvider
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> Now_Sell = () => DateTime.Now;

        public static void SetDateTime(DateTime dateTimeNow)
        {
            Now = () => dateTimeNow;
        }

        public static void ResetDateTime()
        {
            Now = () => DateTime.Now;
        }
        public static void SetDateTime_Sell(DateTime dateTimeNow)
        {
            Now_Sell = () => dateTimeNow;
        }

        public static void ResetDateTime_Sell()
        {
            Now_Sell = () => DateTime.Now;
        }
    }
}
