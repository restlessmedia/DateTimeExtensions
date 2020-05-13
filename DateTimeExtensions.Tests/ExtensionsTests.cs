using restlessmedia.Test;
using System;
using Xunit;

namespace DateExtensions.Tests
{
  public class ExtensionsTests
  {
    public ExtensionsTests()
    {
      DateTime _fixedDate = DateTime.Parse("2020-05-12 15:30:40.1234");
    }

    [Fact]
    public void ToRelativeDate()
    {
      DateTime.Now.AddHours(-1).ToRelativeDate().MustBe("an hour ago");
    }

    [Fact]
    public void IsToday()
    {
      DateTime.Now.IsToday().MustBeTrue();
      DateTime.Now.AddDays(-2).IsToday().MustBeFalse();
    }

    [Fact]
    public void IsWithinDays()
    {
      DateTime.Now.AddDays(1).IsWithinDays(1);
    }

    [Fact]
    public void GetTimestamp()
    {
      DateTime.Parse("2020-05-12 15:30:40.1234").GetTimestamp().MustBe("202005121530401234");
    }

    [Fact]
    public void Between()
    {
      DateTime.Now.Between(DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)).MustBeTrue();
    }

    [Fact]
    public void IsBusinessHour()
    {
      _fixedDate.IsBusinessHour().MustBeTrue();
    }

    private readonly DateTime _fixedDate;
  }
}
