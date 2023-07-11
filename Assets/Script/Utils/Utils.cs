using System;

public static class Utils
{
    public static string GetCommaNum(int num)
    {
        return string.Format("{0:#,0}", num);
    }
    
    public static DateTime GetDateTimeByMilliseconds(long date)
    {
        DateTime resultTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)date) + TimeSpan.FromHours(9);
        return resultTime;
    }
    
    public static long GetMillisecondsByDateTime(DateTime date)
    {
        long resultTime = (long)date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        return resultTime;
    }
}