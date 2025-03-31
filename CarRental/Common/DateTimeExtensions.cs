namespace CarRental.Common;

public static class DateTimeExtensions
{
    public static DateTime RoundToFullHour(this DateTime dateTime)
    {
        return dateTime.AddMinutes(-dateTime.Minute)
            .AddSeconds(-dateTime.Second)
            .AddMilliseconds(-dateTime.Millisecond);
    }
}