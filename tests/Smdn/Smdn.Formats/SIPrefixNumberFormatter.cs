// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class SIPrefixNumberFormatterTests {
    [Test]
    public void TestFormatArgs()
    {
      var provider = SIPrefixNumberFormatter.InvaliantInfo;

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
      var provider = SIPrefixNumberFormatter.InvaliantInfo;

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
      var provider = SIPrefixNumberFormatter.InvaliantInfo;

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
      var provider = SIPrefixNumberFormatter.InvaliantInfo;

      foreach (var pair in new[] {
        new {ExpectedShort = "0B",        ExpectedLong = "0 Bytes",             Value =       0m},
        new {ExpectedShort = "1B",        ExpectedLong = "1 Bytes",             Value =       1m},
        new {ExpectedShort = "10B",       ExpectedLong = "10 Bytes",            Value =      10m},
        new {ExpectedShort = "100B",      ExpectedLong = "100 Bytes",           Value =     100m},
        new {ExpectedShort = "999B",      ExpectedLong = "999 Bytes",           Value =     999m},
        new {ExpectedShort = "1000B",     ExpectedLong = "1000 Bytes",          Value =    1000m},
        new {ExpectedShort = "1001B",     ExpectedLong = "1001 Bytes",          Value =    1001m},
        new {ExpectedShort = "1023B",     ExpectedLong = "1023 Bytes",          Value =    1023m},
        new {ExpectedShort = "1.0kB",     ExpectedLong = "1.0 Kilo Bytes",      Value =    1024m},
        new {ExpectedShort = "1.0kB",     ExpectedLong = "1.0 Kilo Bytes",      Value =    1025m},
        new {ExpectedShort = "10.0kB",    ExpectedLong = "10.0 Kilo Bytes",     Value =   10240m},
        new {ExpectedShort = "100.0kB",   ExpectedLong = "100.0 Kilo Bytes",    Value =  102400m},
        new {ExpectedShort = "1000.0kB",  ExpectedLong = "1000.0 Kilo Bytes",   Value = 1024000m},
        new {ExpectedShort = "1024.0kB",  ExpectedLong = "1024.0 Kilo Bytes",   Value = 1048575m},
        new {ExpectedShort = "1.0MB",     ExpectedLong = "1.0 Mega Bytes",      Value = 1048576m},
        new {ExpectedShort = "10.0MB",    ExpectedLong = "10.0 Mega Bytes",     Value = 10485760m},
        new {ExpectedShort = "100.0MB",   ExpectedLong = "100.0 Mega Bytes",    Value = 104857600m},
        new {ExpectedShort = "1000.0MB",  ExpectedLong = "1000.0 Mega Bytes",   Value = 1048576000m},
        new {ExpectedShort = "1024.0MB",  ExpectedLong = "1024.0 Mega Bytes",   Value = 1073741823m},
        new {ExpectedShort = "1.0GB",     ExpectedLong = "1.0 Giga Bytes",      Value = 1073741824m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:f}", pair.Value));
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:F}", pair.Value));
      }
    }

    [Test]
    public void TestFormatDecimalValue()
    {
      var provider = SIPrefixNumberFormatter.InvaliantInfo;
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

          new {Expected = "976.6kB",          Format = "f"},
          new {Expected = "976.6 Kilo Bytes", Format = "F"},
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
      var provider = SIPrefixNumberFormatter.InvaliantInfo;
      var binaryValue = +1048576m;

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

          new {Expected = "1.0MB",          Format = "f"},
          new {Expected = "1.0 Mega Bytes", Format = "F"},
        }) {
          var format = string.Format("{{0:{0}}}", pair.Format);
          var expected = (sign < decimal.Zero) ? "-" + pair.Expected : pair.Expected;

          Assert.AreEqual(expected, string.Format(provider, format, sign * binaryValue));
        }
      }
    }

    [Test]
    public void TestFormatSpecificLocaleJA()
    {
      var provider = new SIPrefixNumberFormatter(new CultureInfo("ja-jp"));

      foreach (var pair in new[] {
        new {ExpectedShort = "1.0",       ExpectedLong = "1.0",           Value =             1m},
        new {ExpectedShort = "1.0ki",     ExpectedLong = "1.0 キビ",      Value =          1024m},
        new {ExpectedShort = "1.0Mi",     ExpectedLong = "1.0 メビ",      Value =       1048576m},
        new {ExpectedShort = "1.0Gi",     ExpectedLong = "1.0 ギビ",      Value =    1073741824m},
        new {ExpectedShort = "1.0Ti",     ExpectedLong = "1.0 テビ",      Value = 1099511627776m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:b1}", pair.Value));
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:B1}", pair.Value));
      }

      foreach (var pair in new[] {
        new {ExpectedShort = "1.0",       ExpectedLong = "1.0",           Value =             1m},
        new {ExpectedShort = "1.0k",      ExpectedLong = "1.0 キロ",      Value =          1000m},
        new {ExpectedShort = "1.0M",      ExpectedLong = "1.0 メガ",      Value =       1000000m},
        new {ExpectedShort = "1.0G",      ExpectedLong = "1.0 ギガ",      Value =    1000000000m},
        new {ExpectedShort = "1.0T",      ExpectedLong = "1.0 テラ",      Value = 1000000000000m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:d1}", pair.Value));
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:D1}", pair.Value));
      }

      foreach (var pair in new[] {
        new {ExpectedShort = "1B",        ExpectedLong = "1バイト",              Value =             1m},
        new {ExpectedShort = "1.0kB",     ExpectedLong = "1.0 キロバイト",       Value =          1024m},
        new {ExpectedShort = "1.0MB",     ExpectedLong = "1.0 メガバイト",       Value =       1048576m},
        new {ExpectedShort = "1.0GB",     ExpectedLong = "1.0 ギガバイト",       Value =    1073741824m},
        new {ExpectedShort = "1.0TB",     ExpectedLong = "1.0 テラバイト",       Value = 1099511627776m},
      }) {
        Assert.AreEqual(pair.ExpectedShort, string.Format(provider, "{0:f}", pair.Value));
        Assert.AreEqual(pair.ExpectedLong,  string.Format(provider, "{0:F}", pair.Value));
      }
    }

    [Test]
    public void TestReadOnlyInstanceThrowsException()
    {
      foreach (var provider in new[] {
        SIPrefixNumberFormatter.InvaliantInfo,
        SIPrefixNumberFormatter.CurrentInfo,
      }) {
        Assert.IsTrue(provider.IsReadOnly, "IsReadOnly");

        foreach (var pair in new[] {
          new {Name = "ByteUnit", Action = (Action<string>)delegate(string arg) {provider.ByteUnit = arg;}},
          new {Name = "ByteUnitAbbreviation", Action = (Action<string>)delegate(string arg) {provider.ByteUnitAbbreviation = arg;}},
          new {Name = "PrefixUnitDelimiter", Action = (Action<string>)delegate(string arg) {provider.PrefixUnitDelimiter = arg;}},
          new {Name = "ValuePrefixDelimiter", Action = (Action<string>)delegate(string arg) {provider.ValuePrefixDelimiter = arg;}},
        }) {
          Assert.Throws<InvalidOperationException>(() => pair.Action(string.Empty), pair.Name);
        }
      }
    }

    [Test]
    public void TestFormatWithCustomProvider()
    {
      var provider = new SIPrefixNumberFormatter();

      Assert.IsFalse(provider.IsReadOnly, "IsReadOnly");

      provider.ByteUnitAbbreviation = "BYTE";

      foreach (var pair in new[] {
        new {Expected = "1BYTE",    Value =             1m},
        new {Expected = "1.0kBYTE", Value =          1024m},
        new {Expected = "1.0MBYTE", Value =       1048576m},
        new {Expected = "1.0GBYTE", Value =    1073741824m},
        new {Expected = "1.0TBYTE", Value = 1099511627776m},
      }) {
        Assert.AreEqual(pair.Expected, string.Format(provider, "{0:f}", pair.Value));
      }
    }
  }
}
