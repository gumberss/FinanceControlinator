using System;

namespace FinanceControlinator.Common.Utils
{
    public interface IDateService
    {
        (DateTime startDate, DateTime endDate) StartAndEndMonthDate(DateTime date);

        DateTime FirstMonthDayDate(DateTime date);
    }
    public class DateService : IDateService
    {
        public DateTime FirstMonthDayDate(DateTime date)
            => date.AddDays(-(date.Day - 1));

        public (DateTime startDate, DateTime endDate) StartAndEndMonthDate(DateTime date)
        {
            var lastMonthDay = DateTime.DaysInMonth(date.Year, date.Month);

            return
                (FirstMonthDayDate(date),
                new DateTime(date.Year, date.Month, lastMonthDay));
        }
    }
}
