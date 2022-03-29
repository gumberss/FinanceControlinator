using System;

namespace FinanceControlinator.Common.Utils
{
    public interface IDateService
    {
        (DateTime startDate, DateTime endDate) StartAndEndMonthDate(DateTime date);
    }
    public class DateService : IDateService
    {
        public (DateTime startDate, DateTime endDate) StartAndEndMonthDate(DateTime date)
        {
            var firstMonthDay = 1;
            var lastMonthDay = DateTime.DaysInMonth(date.Year, date.Month);

            return
                (new DateTime(date.Year, date.Month, firstMonthDay),
                new DateTime(date.Year, date.Month, lastMonthDay));
        }
    }
}
