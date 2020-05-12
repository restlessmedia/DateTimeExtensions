using System.Collections.Generic;
using System.Linq;

namespace System
{
  public static class Extensions
  {
    /// <summary>
    /// Returns a 'pretty' relative date string i.e. "less than a minute" "about a year" etc
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToRelativeDate(this DateTime input)
    {
      TimeSpan diff = DateTime.Now.Subtract(input);
      double totalMinutes = diff.TotalMinutes;
      string suffix = " ago";

      if (totalMinutes < 0.0)
      {
        totalMinutes = Math.Abs(totalMinutes);
        suffix = " from now";
      }

      return string.Concat(_descriptors.First(n => totalMinutes < n.UpperBound).DescriptionGenerator(totalMinutes), suffix);
    }

    /// <summary>
    /// Returns true/false on whether the input datetime is on the same year, month and day as today
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsToday(this DateTime input)
    {
      return input.Date == DateTime.Today;
    }

    /// <summary>
    /// If true, the specified date has fallen within the last n days
    /// </summary>
    /// <param name="input"></param>
    /// <param name="days"></param>
    /// <returns></returns>
    public static bool IsWithinDays(this DateTime input, int days)
    {
      return input.Between(DateTime.Today.AddDays(days * -1), DateTime.Today);
    }

    /// <summary>
    /// Returns a timestamp for the given date time object.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string GetTimestamp(this DateTime input, string format = _defaultTimestampFormat)
    {
      return input.ToString(format);
    }

    /// <summary>
    /// Returns true if the date falls between the specified dates
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minDate"></param>
    /// <param name="maxDate"></param>
    /// <returns></returns>
    public static bool Between(this DateTime value, DateTime minDate, DateTime maxDate)
    {
      return value >= minDate && value <= maxDate;
    }

    /// <summary>
    /// Returns true if the date time value falls within normal business hours 9-6pm
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsBusinessHour(this DateTime value, int startHour = 9, int endHour = 18)
    {
      return value.IsWeekday() && (value.Hour >= startHour && value.Hour < endHour);
    }

    /// <summary>
    /// Returns true if the date falls Monday, Tuesday, Wednesday, Thursday or Friday
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsWeekday(this DateTime value)
    {
      int day = (int)value.DayOfWeek;
      return day > 0 && day < 6;
    }

    /// <summary>
    /// Returns the next hour of the day or if that falls in the current hour, the point in time tomorrow.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hour"></param>
    /// <returns></returns>
    public static DateTime NextHour(this DateTime value, int hour)
    {
      return value.Date.AddHours(hour).AddDays(value.Hour >= hour ? 1 : 0);
    }

    public static DateTime Next(this DateTime value, Frequency frequency, int iterator)
    {
      switch (frequency)
      {
        case Frequency.Second:
          {
            return value.AddSeconds(iterator);
          }
        case Frequency.Minute:
          {
            return value.AddMinutes(iterator);
          }
        case Frequency.Hourly:
          {
            return value.AddHours(iterator);
          }
        case Frequency.AtHour:
          {
            return value.NextHour(iterator);
          }
        case Frequency.Daily:
          {
            return value.AddDays(iterator);
          }
        case Frequency.Weekly:
          {
            return value.AddDays(7 * iterator);
          }
        case Frequency.Monthly:
          {
            return value.AddMonths(iterator);
          }
        case Frequency.Yearly:
          {
            return value.AddYears(iterator);
          }
        default:
          throw new ArgumentException($"Next not available for frequency '{frequency}'");
      }
    }

    internal class TimeSpanDescriptor
    {
      public TimeSpanDescriptor(double upperBound, Func<double, string> descriptionGenerator)
      {
        UpperBound = upperBound;
        DescriptionGenerator = descriptionGenerator;
      }

      public double UpperBound { get; private set; }

      public Func<double, string> DescriptionGenerator { get; private set; }
    }

    private static IEnumerable<TimeSpanDescriptor> _descriptors = new[]
    {
        new TimeSpanDescriptor(0.75, mins => "a minute"),
        new TimeSpanDescriptor(1.5, mins => "a minute"),
        new TimeSpanDescriptor(45, mins => $"{Math.Round(mins)} minutes"),
        new TimeSpanDescriptor(90, mins => "an hour"),
        new TimeSpanDescriptor(60 * 24, mins => $"{Math.Round(Math.Abs(mins / 60))} hours"),
        new TimeSpanDescriptor(60 * 48, mins => "a day"),
        new TimeSpanDescriptor(60 * 24 * 30, mins => $"{Math.Floor(Math.Abs(mins / 1440))} days"),
        new TimeSpanDescriptor(60 * 24 * 60, mins => "a month"),
        new TimeSpanDescriptor(60 * 24 * 365, mins => $"{Math.Floor(Math.Abs(mins / 43200))} months"),
        new TimeSpanDescriptor(60 * 24 * 365 * 2, mins => "a year"),
        new TimeSpanDescriptor(double.MaxValue, mins => $"{Math.Floor(Math.Abs(mins / 525600))} years"),
    };

    private const string _defaultTimestampFormat = "yyyyMMddHHmmssffff";
  }
}