using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class SIPrefixFormatTests {
    [Test]
    public void TestFormatArgs()
    {
      var provider = new SIPrefixFormat();

      foreach (var arg in new object[] {
        1000,
        1000L,
        1000.0f,
        1000.0,
        1000.0m,
        "1000.0",
      }) {
        Assert.AreEqual("1k", string.Format(provider, "{0:d}", arg), arg.GetType().ToString());
      }

      foreach (var arg in new object[] {
        "aaaa",
        new Guid(),
        new object(),
      }) {
        Assert.AreNotEqual("1k", string.Format(provider, "{0:d}", arg), arg.GetType().ToString());
      }
    }

    [Test]
    public void TestFormatDecimal()
    {
      var provider = new SIPrefixFormat();

      foreach (var pair in new[] {
        new {ExpectedShort = "0",     ExpectedLong ="0",        Value =       0m},
        new {ExpectedShort = "999",   ExpectedLong ="999",      Value =     999m},
        new {ExpectedShort = "1k",    ExpectedLong ="1 Kilo",   Value =    1000m},
        new {ExpectedShort = "1k",    ExpectedLong ="1 Kilo",   Value =    1001m},
        new {ExpectedShort = "1k",    ExpectedLong ="1 Kilo",   Value =    1023m},
        new {ExpectedShort = "1k",    ExpectedLong ="1 Kilo",   Value =    1024m},
        new {ExpectedShort = "1k",    ExpectedLong ="1 Kilo",   Value =    1025m},
        new {ExpectedShort = "999k",  ExpectedLong ="999 Kilo", Value =  999999m},
        new {ExpectedShort = "1M",    ExpectedLong ="1 Mega",   Value = 1000000m},
        new {ExpectedShort = "1M",    ExpectedLong ="1 Mega",   Value = 1000001m},
        new {ExpectedShort = "1M",    ExpectedLong ="1 Mega",   Value = 1048575m},
        new {ExpectedShort = "1M",    ExpectedLong ="1 Mega",   Value = 1048576m},
        new {ExpectedShort = "1M",    ExpectedLong ="1 Mega",   Value = 1048577m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:d}", pair.Value), "short form");
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:D}", pair.Value), "long form");
      }
    }

    [Test]
    public void TestFormatBinary()
    {
      var provider = new SIPrefixFormat();

      foreach (var pair in new[] {
        new {ExpectedShort = "0",      ExpectedLong = "0",          Value =       0m},
        new {ExpectedShort = "999",    ExpectedLong = "999",        Value =     999m},
        new {ExpectedShort = "1000",   ExpectedLong = "1000",       Value =    1000m},
        new {ExpectedShort = "1001",   ExpectedLong = "1001",       Value =    1001m},
        new {ExpectedShort = "1023",   ExpectedLong = "1023",       Value =    1023m},
        new {ExpectedShort = "1ki",    ExpectedLong = "1 Kibi",     Value =    1024m},
        new {ExpectedShort = "1ki",    ExpectedLong = "1 Kibi",     Value =    1025m},
        new {ExpectedShort = "976ki",  ExpectedLong = "976 Kibi",   Value =  999999m},
        new {ExpectedShort = "976ki",  ExpectedLong = "976 Kibi",   Value = 1000000m},
        new {ExpectedShort = "976ki",  ExpectedLong = "976 Kibi",   Value = 1000001m},
        new {ExpectedShort = "1023ki", ExpectedLong = "1023 Kibi",  Value = 1048575m},
        new {ExpectedShort = "1Mi",    ExpectedLong = "1 Mebi",     Value = 1048576m},
        new {ExpectedShort = "1Mi",    ExpectedLong = "1 Mebi",     Value = 1048577m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:b}", pair.Value), "short form");
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:B}", pair.Value), "long form");
      }
    }

    [Test]
    public void TestFormatFileSize()
    {
      var provider = new SIPrefixFormat();

      foreach (var pair in new[] {
        new {ExpectedShort = "0",       ExpectedLong = "0",             Value =       0m},
        new {ExpectedShort = "1",       ExpectedLong = "1",             Value =       1m},
        new {ExpectedShort = "10",      ExpectedLong = "10",            Value =      10m},
        new {ExpectedShort = "100",     ExpectedLong = "100",           Value =     100m},
        new {ExpectedShort = "999",     ExpectedLong = "999",           Value =     999m},
        new {ExpectedShort = "1000",    ExpectedLong = "1000",          Value =    1000m},
        new {ExpectedShort = "1001",    ExpectedLong = "1001",          Value =    1001m},
        new {ExpectedShort = "1023",    ExpectedLong = "1023",          Value =    1023m},
        new {ExpectedShort = "1.0k",    ExpectedLong = "1.0 Kilo",      Value =    1024m},
        new {ExpectedShort = "1.0k",    ExpectedLong = "1.0 Kilo",      Value =    1025m},
        new {ExpectedShort = "10.0k",   ExpectedLong = "10.0 Kilo",     Value =   10240m},
        new {ExpectedShort = "100.0k",  ExpectedLong = "100.0 Kilo",    Value =  102400m},
        new {ExpectedShort = "1000.0k", ExpectedLong = "1000.0 Kilo",   Value = 1024000m},
        new {ExpectedShort = "1024.0k", ExpectedLong = "1024.0 Kilo",   Value = 1048575m},
        new {ExpectedShort = "1.0M",    ExpectedLong = "1.0 Mega",      Value = 1048576m},
        new {ExpectedShort = "10.0M",   ExpectedLong = "10.0 Mega",     Value = 10485760m},
        new {ExpectedShort = "100.0M",  ExpectedLong = "100.0 Mega",    Value = 104857600m},
        new {ExpectedShort = "1000.0M", ExpectedLong = "1000.0 Mega",   Value = 1048576000m},
        new {ExpectedShort = "1024.0M", ExpectedLong = "1024.0 Mega",   Value = 1073741823m},
        new {ExpectedShort = "1.0G",    ExpectedLong = "1.0 Giga",      Value = 1073741824m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:f}", pair.Value));
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:F}", pair.Value));
      }
    }

    [Test]
    public void TestFormatDecimalValue()
    {
      var provider = new SIPrefixFormat();
      var decimalValue = +1000000m;

      foreach (var sign in new[] {+1m, -1m}) {
        foreach (var pair in new[] {
          new {Expected = "976ki",            Format = "b0"},
          new {Expected = "976.563ki",        Format = "b3"},
          new {Expected = "976.562500ki",     Format = "b6"},
          new {Expected = "976 Kibi",         Format = "B0"},
          new {Expected = "976.563 Kibi",     Format = "B3"},
          new {Expected = "976.562500 Kibi",  Format = "B6"},

          new {Expected = "1M",             Format = "d0"},
          new {Expected = "1.000M",         Format = "d3"},
          new {Expected = "1.000000M",      Format = "d6"},
          new {Expected = "1 Mega",         Format = "D0"},
          new {Expected = "1.000 Mega",     Format = "D3"},
          new {Expected = "1.000000 Mega",  Format = "D6"},

          new {Expected = "976.6k",         Format = "f"},
          new {Expected = "976.6 Kilo",     Format = "F"},
        }) {
          var format = string.Format("{{0:{0}}}", pair.Format);
          var expected = (sign < decimal.Zero) ? "-" + pair.Expected : pair.Expected;

          Assert.AreEqual(expected, string.Format(provider, format, sign * decimalValue));
        }
      }
    }

    [Test]
    public void TestFormatBinaryValue()
    {
      var binaryValue = +1048576m;
      var provider = new SIPrefixFormat();

      foreach (var sign in new[] {+1m, -1m}) {
        foreach (var pair in new[] {
          new {Expected = "1Mi",            Format = "b0"},
          new {Expected = "1.000Mi",        Format = "b3"},
          new {Expected = "1.000000Mi",     Format = "b6"},
          new {Expected = "1 Mebi",         Format = "B0"},
          new {Expected = "1.000 Mebi",     Format = "B3"},
          new {Expected = "1.000000 Mebi",  Format = "B6"},

          new {Expected = "1M",             Format = "d0"},
          new {Expected = "1.049M",         Format = "d3"},
          new {Expected = "1.048576M",      Format = "d6"},
          new {Expected = "1 Mega",         Format = "D0"},
          new {Expected = "1.049 Mega",     Format = "D3"},
          new {Expected = "1.048576 Mega",  Format = "D6"},

          new {Expected = "1.0M",           Format = "f"},
          new {Expected = "1.0 Mega",       Format = "F"},
        }) {
          var format = string.Format("{{0:{0}}}", pair.Format);
          var expected = (sign < decimal.Zero) ? "-" + pair.Expected : pair.Expected;

          Assert.AreEqual(expected, string.Format(provider, format, sign * binaryValue));
        }
      }
    }
  }
}
