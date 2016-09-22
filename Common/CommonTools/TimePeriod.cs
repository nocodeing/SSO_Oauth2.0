using System;

namespace CommonTools
{
    public class TimePeriod
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public override bool Equals(object obj)
        {
            var timePeriod = obj as TimePeriod;
            return (null != timePeriod && timePeriod.StartTime == StartTime && timePeriod.EndTime == EndTime);
        }

        public override int GetHashCode()
        {
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
    }
}