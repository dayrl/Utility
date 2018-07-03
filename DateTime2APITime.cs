namespace Zdd.Utility
{
    using System;

    /********************************************************************
	file base:	DateTime2APITime
	file ext:	    cs
	author:		
	purpose:	    DateTime和Long时间的转换
    *********************************************************************/
    public class DateTime2APITime
    {
        public static long DateTime2Long(DateTime dTime)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = TimeZone.CurrentTimeZone.ToLocalTime(dt);
            TimeSpan ts1 = new TimeSpan(dTime.Ticks);
            TimeSpan ts2 = new TimeSpan(dt.Ticks);
            return (long)(ts1.Subtract(ts2).TotalSeconds);
        }

        public static DateTime Long2DateTime(long lTime)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(lTime);
            return TimeZone.CurrentTimeZone.ToLocalTime(dt);
        }
    }
}
