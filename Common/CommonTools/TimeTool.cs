using System;

namespace CommonTools
{
    public static class TimeTool
    {
        public static long ToLongUtc(this DateTime dateTime)
        {
            // dateTime = dateTime.ToUniversalTime();
            var zoneDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)dateTime.Subtract(zoneDate).TotalMilliseconds;
        }
        public static double ToDoubleUtc(this DateTime dateTime)
        {
            // dateTime = dateTime.ToUniversalTime();
            var zoneDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return dateTime.Subtract(zoneDate).TotalMilliseconds;
        }
        public static long GetJavascriptTimestamp(this DateTime input)
        {
            var span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            var time = input.Subtract(span);
            return (long)(time.Ticks / 10000);
        }
        public static TimePeriod ToDayPeriod(this DateTime dateTime)
        {
            var timePeriod = new TimePeriod { StartTime = dateTime.Date };
            timePeriod.EndTime = timePeriod.StartTime.AddDays(1).AddSeconds(-1);
            return timePeriod;
        }
        public static TimePeriod ToMonthPeriod(this DateTime dateTime)
        {
            var timePeriod = new TimePeriod { StartTime = dateTime.Date.AddDays(-dateTime.Day + 1) };
            timePeriod.EndTime = timePeriod.StartTime.AddMonths(1).AddMilliseconds(-1);
            return timePeriod;
        }
        public static TimePeriod ToWeekPeriod(this DateTime dateTime)
        {
            var week = (int)dateTime.DayOfWeek % 7;
            var startTime = dateTime.AddDays(-week).Date;
            var timePeriod = new TimePeriod
            {
                StartTime = startTime,
                EndTime = startTime.AddDays(7).ToDayPeriod().EndTime
            };
            return timePeriod;
        }
        public static bool IsSameByPart(this DateTime baseDate, DateTimePart part, DateTime date)
        {
            var result = false;
            switch (part)
            {
                case DateTimePart.年:
                    if (baseDate.Year == date.Year)
                        result = true;
                    break;
                case DateTimePart.月:
                    if (baseDate.Year == date.Year && baseDate.Month == date.Month)
                        result = true;
                    break;
                case DateTimePart.日:
                    if (baseDate.IsSameByPart(DateTimePart.月, date) && baseDate.Day == date.Day)
                        result = true;
                    break;
                case DateTimePart.时:
                    if (baseDate.IsSameByPart(DateTimePart.日, date) && baseDate.Hour == date.Hour)
                        result = true;
                    break;
                case DateTimePart.分:
                    if (baseDate.IsSameByPart(DateTimePart.时, date) && baseDate.Minute == date.Minute)
                        result = true;
                    break;
                case DateTimePart.秒:
                    if (baseDate.IsSameByPart(DateTimePart.分, date) && baseDate.Second == date.Second)
                        result = true;
                    break;
            }
            return result;
        }
        //如10点50分 格式为10.50转化为为时间数字格式
        public static double ToTime(this Double time)
        {
            var num1 = (int)time;
            var num2 = time - num1;
            var num3 = num2 / 0.6d;
            return num1 + num3;

        }
        //时间转化为数字
        public static double GetNumberTime(this DateTime sourceTime)
        {
            var time = Convert.ToDouble(String.Format("{0}.{1}", sourceTime.Hour, sourceTime.Minute));
            return time.ToTime();
        }

        //分钟转化为（天、小时、分钟格式）
        public static string GetDateFormat(this int minutes)
        {
            var date = new DateTime(1990, 1, 1, 0, 0, 0);
            date = date.AddMinutes(minutes);
            var day = (date.Day - 1) == 0 ? "" : (date.Day - 1 + "天");
            var hour = date.Hour == 0 ? "" : date.Hour + "小时";
            var minute = date.Minute == 0 ? "" : date.Minute + "分钟";
            return String.Format("{0}{1}{2}", day, hour, minute);
        }

        public static string ToLocalTime(this DateTime? date)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm}", date ?? default(DateTime));
        }
        public static string ToLocalTime(this DateTime date)
        {
            return String.Format("{0:yyyy-MM-dd HH:mm}", date);
        }
        public static string ToLocalTime(this DateTime date, string fmt)
        {
            return String.Format(fmt, date);
        }
        public static string GetDayPart(this DateTime dateTime)
        {
            var hour = dateTime.Hour;
            if (hour >= 8 && hour <= 12)
            {
                return "上午";
            }
            else if (hour > 12 && hour <= 18)
            {
                return "下午";
            }
            else if (hour >= 18 && hour < 24)
            {
                return "晚上";
            }
            else if (hour >= 0 && hour < 8)
            {
                return "早上";
            }
            return "其他";
        }

        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
    }

    public enum DateTimePart
    {
        年,
        月,
        日,
        时,
        分,
        秒
    }
}
