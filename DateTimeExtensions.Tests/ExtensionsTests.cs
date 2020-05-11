using restlessmedia.Test;
using System;
using Xunit;

namespace DateExtensions.Tests
{
  public class ExtensionsTests
  {
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
  }
}
