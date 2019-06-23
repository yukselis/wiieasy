using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Arwend
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Tarih değerinin uygunluğunu kontrol eder ve uygun olmayan değerler için sql server'ın alt sınır tarih değerini döner.
        /// </summary>
        public static DateTime ValidateDate(this DateTime? datetime)
        {
            DateTime minValue = new DateTime(1753, 1, 1);
            return (!datetime.HasValue || datetime.Value <= minValue) ? minValue : datetime.Value;
        }

        /// <summary>
        /// Verilen tarih değerini '23 Ekim 1988 Çarşamba' formatında string ifadeye çevirir. 
        /// </summary>
        public static string ToTrString(this DateTime datetime, string regex = "dd MMM yyyy, dddd")
        {
            return datetime.ToString(regex, new CultureInfo("tr-TR"));
        }
        public static string AsFileName(this DateTime datetime, string baseFileName = "")
        {
            if (string.IsNullOrEmpty(baseFileName)) return datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            return string.Format("{0}-{1}", baseFileName.ToSlug(), datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture));
        }
        public static string AsDirectoryName(this DateTime datetime, string regex = "yyyy-MM-dd")
        {
            return datetime.ToString(regex, CultureInfo.InvariantCulture);
        }
        public static DateTime FirstDateOfWeek(this DateTime datetime, int year, int weekOfYear)
        {
            DateTime firstDay = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - firstDay.DayOfWeek;

            DateTime firstThursday = firstDay.AddDays(daysOffset);
            var calendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            int firstWeek = calendar.GetWeekOfYear(firstThursday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNumber = weekOfYear;
            if (firstWeek <= 1) weekNumber -= 1;

            datetime = firstThursday.AddDays(weekNumber * 7);
            return datetime.AddDays(-3);
        }

        public static DateTime Trim(this DateTime datetime, long roundTicks)
        {
            return new DateTime(datetime.Ticks - datetime.Ticks % roundTicks);
        }

        public static DateTime TrimTime(this DateTime datetime)
        {
            return datetime.Trim(TimeSpan.TicksPerDay);
        }

        public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval, MidpointRounding roundingType)
        {
            return new TimeSpan(
                Convert.ToInt64(Math.Round(
                    time.Ticks / (decimal)roundingInterval.Ticks,
                    roundingType
                )) * roundingInterval.Ticks
            );
        }

        public static TimeSpan Round(this TimeSpan time, TimeSpan roundingInterval)
        {
            return Round(time, roundingInterval, MidpointRounding.AwayFromZero);
        }

        public static DateTime Round(this DateTime datetime, TimeSpan roundingInterval)
        {
            return new DateTime((datetime - DateTime.MinValue).Round(roundingInterval).Ticks);
        }

        public static DateTime RoundMinute(this DateTime datetime, int accumulator)
        {
            var result = datetime.Round(TimeSpan.FromMinutes(accumulator));
            if (result < datetime)
                result = result.AddMinutes(accumulator);
            return result;
        }
        public static double UnixTicks(this DateTime datetime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1);
            DateTime universal = datetime.ToUniversalTime();
            TimeSpan timespan = new TimeSpan(universal.Ticks - unixStart.Ticks);
            return timespan.TotalMilliseconds;
        }
    }
}
