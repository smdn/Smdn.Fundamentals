// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public partial class UInt48Tests {
    [Test]
    public void TestSizeOfStructure()
    {
      Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(UInt48)), Is.EqualTo(6));
    }

    [Test]
    public void TestConstruct()
    {
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, isBigEndian: true), Is.EqualTo((UInt48)0xff0000000000));
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true), Is.EqualTo((UInt48)0xff0000000000));
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, isBigEndian: true), Is.EqualTo((UInt48)0x0000000000cc));
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, isBigEndian: false), Is.EqualTo((UInt48)0x0000000000ff));
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: false), Is.EqualTo((UInt48)0x0000000000ff));
      Assert.That(new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, isBigEndian: false), Is.EqualTo((UInt48)0xcc0000000000));

      Assert.Throws<ArgumentNullException>(() => new UInt48(null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, -1));
      Assert.Throws<ArgumentException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 0));
      Assert.Throws<ArgumentException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 1));
    }

    [Test]
    public void TestConstruct_ReadOnlySpan()
    {
      Assert.That(
        new UInt48(stackalloc byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, isBigEndian: true),
        Is.EqualTo((UInt48)0x0123456789AB)
      );
      Assert.That(
        new UInt48(stackalloc byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, isBigEndian: false),
        Is.EqualTo((UInt48)0xAB8967452301)
      );

      Assert.Throws<ArgumentException>(() => new UInt48(ReadOnlySpan<byte>.Empty));
      Assert.Throws<ArgumentException>(() => new UInt48(stackalloc byte[] { 0x00 }));
      Assert.Throws<ArgumentException>(() => new UInt48(stackalloc byte[] { 0x00, 0x00 }));
      Assert.Throws<ArgumentException>(() => new UInt48(stackalloc byte[] { 0x00, 0x00, 0x00 }));
      Assert.Throws<ArgumentException>(() => new UInt48(stackalloc byte[] { 0x00, 0x00, 0x00, 0x00 }));
      Assert.Throws<ArgumentException>(() => new UInt48(stackalloc byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }));
    }

    [Test]
    public void Test()
    {
      Assert.That((UInt48)(long)0x00ff00ff, Is.EqualTo((UInt48)(int)0x00ff00ff));

      var val = (UInt48)(long)0x123456789012;

      Assert.That(val.ToInt64(), Is.EqualTo((long)0x123456789012));

      var zero = (UInt48)0x000000000000;
      var one  = (UInt48)0x000000000001;

      Assert.That(zero.Equals(zero), Is.True);
      Assert.That(zero.Equals(one), Is.False);
      Assert.That(zero.Equals(null), Is.False);
    }

    [Test]
    public void TestConstants()
    {
      Assert.That(UInt48.Zero, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)), nameof(UInt48.Zero));
      Assert.That(UInt48.One, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isBigEndian: true)), nameof(UInt48.One));
      Assert.That(UInt48.MinValue, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)), nameof(UInt48.MinValue));
      Assert.That(UInt48.MaxValue, Is.EqualTo(new UInt48(stackalloc byte[6] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, isBigEndian: true)), nameof(UInt48.MaxValue));
    }

    [Test]
    public void TestEquals()
    {
      Assert.That(UInt48.Zero.Equals(UInt48.Zero), Is.True);
      Assert.That(UInt48.Zero.Equals(0L), Is.True);
      Assert.That(UInt48.Zero.Equals(0UL), Is.True);
      Assert.That(UInt48.One.Equals(UInt48.Zero), Is.False);
      Assert.That(UInt48.One.Equals(0L), Is.False);
      Assert.That(UInt48.One.Equals(0UL), Is.False);
      Assert.That(UInt48.Zero.Equals(UInt48.MaxValue), Is.False);
      Assert.That(UInt48.Zero.Equals(long.MaxValue), Is.False);
      Assert.That(UInt48.Zero.Equals(ulong.MaxValue), Is.False);

      object val;

      val = UInt48.Zero; Assert.That(UInt48.Zero.Equals(val), Is.True);
      val = null; Assert.That(UInt48.Zero.Equals(val), Is.False);
    }

    [TestCase(0L, true)]
    [TestCase(0uL, true)]
    [TestCase((object)null, false)]
    [TestCase(true, false)]
    public void TestEquals_Object(object value, bool expected)
      => Assert.That(UInt48.Zero.Equals(value), Is.EqualTo(expected));

    [Test]
    public void TestCompareTo()
    {
      var zero = (UInt48)0x000000000000;
      var one  = (UInt48)0x000000000001;

      Assert.That(zero.CompareTo(null), Is.EqualTo(1), "CompareTo(null)");

      Assert.That(0 == zero.CompareTo(zero), "Zero.CompareTo(Zero)");
      Assert.That(0 > zero.CompareTo(one), "Zero.CompareTo(Zero)");
      Assert.That(0 < one.CompareTo(zero), "One.CompareTo(Zero)");
      Assert.That(0 == one.CompareTo(one), "One.CompareTo(One)");

      Assert.That(0 < one.CompareTo(0L), "One.CompareTo(0)");
      Assert.That(0 < one.CompareTo(0uL), "One.CompareTo(0u)");
    }

    [TestCase(false)]
    [TestCase(0.0)]
    [TestCase((byte)0)]
    [TestCase("0")]
    public void TestCompareTo_NotComparable(object value)
      => Assert.Throws<ArgumentException>(() => UInt48.Zero.CompareTo(value));

    [Test]
    public void TestOpExplicitFromInt32()
    {
      foreach (var test in new[] {
        new {Value = (int)0x00000000, ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (int)0x7fffffff, ExpectedResult = (long)0x000000007fffffff, ExpectedHex = "7fffffff"},
        new {Value = Int32.MaxValue,  ExpectedResult = (long)0x000000007fffffff, ExpectedHex = "7fffffff"},
        new {Value = (int)-1,         ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = Int32.MinValue,  ExpectedResult = (long)0x0000ffff80000000, ExpectedHex = "ffff80000000"},
      }) {

        try {
          UInt48 val = unchecked((UInt48)test.Value);

          Assert.That(val.ToInt64(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
          Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedFromInt32()
    {
      foreach (var test in new[] {
        new {Value = (int)-1},
        new {Value = Int32.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = checked((UInt48)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromInt64()
    {
      foreach (var test in new[] {
        new {Value = (long)0x0000000000000000, ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (long)0x0000ffffffffffff, ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = (long)-1,                 ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = (long)0x0001000000000000, ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = Int64.MaxValue,           ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = Int64.MinValue,           ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
      }) {

        try {
          UInt48 val = unchecked((UInt48)test.Value);

          Assert.That(val.ToInt64(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
          Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedFromInt64()
    {
      foreach (var test in new[] {
        new {Value = (long)-1},
        new {Value = (long)0x0001000000000000},
        new {Value = Int64.MaxValue},
        new {Value = Int64.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = checked((UInt48)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt64()
    {
      foreach (var test in new[] {
        new {Value = (ulong)0x0000000000000000,   ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (ulong)0x0000ffffffffffff,   ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = UInt64.MinValue,             ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (ulong)0x0001000000000000,   ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (ulong)0xffffffffffffffff,   ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = UInt64.MaxValue,             ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
      }) {
        try {
          UInt48 val = unchecked((UInt48)test.Value);

          Assert.That(val.ToInt64(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
          Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedFromUInt64()
    {
      foreach (var test in new[] {
        new {Value = (ulong)0x0001000000000000},
        new {Value = (ulong)0xffffffffffffffff},
        new {Value = UInt64.MaxValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = checked((UInt48)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitToInt32()
    {
      foreach (var test in new[] {
        new {Value = UInt48.MinValue,         ExpectedResult = (int)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x000000000000,  ExpectedResult = (int)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x00007fffffff,  ExpectedResult = (int)0x7fffffff, ExpectedHex = "7fffffff"},
        new {Value = (UInt48)0x000080000000,  ExpectedResult = unchecked((int)(uint)0x80000000), ExpectedHex = "80000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedResult = unchecked((int)(uint)0xffffffff), ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedResult = unchecked((int)(uint)0xffffffff), ExpectedHex = "ffffffffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        try {
          Assert.That(unchecked((int)test.Value), Is.EqualTo(test.ExpectedResult));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedToInt32()
    {
      foreach (var test in new[] {
        new {Value = (UInt48)0x000080000000,  ExpectedHex = "80000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedHex = "ffffffffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        Assert.Throws<OverflowException>(() => { var i = checked((int)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitToUInt32()
    {
      foreach (var test in new[] {
        new {Value = UInt48.MinValue,         ExpectedResult = (uint)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x000000000000,  ExpectedResult = (uint)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x00007fffffff,  ExpectedResult = (uint)0x7fffffff, ExpectedHex = "7fffffff"},
        new {Value = (UInt48)0x000080000000,  ExpectedResult = (uint)0x80000000, ExpectedHex = "80000000"},
        new {Value = (UInt48)0x0000ffffffff,  ExpectedResult = (uint)0xffffffff, ExpectedHex = "ffffffff"},
        new {Value = (UInt48)0x000100000000,  ExpectedResult = (uint)0x00000000, ExpectedHex = "100000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedResult = (uint)0xffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedResult = (uint)0xffffffff, ExpectedHex = "ffffffffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        try {
          Assert.That(unchecked((uint)test.Value), Is.EqualTo(test.ExpectedResult));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedToUInt32()
    {
      foreach (var test in new[] {
        new {Value = (UInt48)0x000100000000,  ExpectedHex = "100000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedHex = "ffffffffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        Assert.Throws<OverflowException>(() => { var ui = checked((uint)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt32()
    {
      UInt48 val;

      foreach (var test in new[] {
        new {Value = (uint)0,           ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (uint)0x00000000,  ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (uint)0xffffffff,  ExpectedResult = (long)0x00000000ffffffff, ExpectedHex = "ffffffff"},
        new {Value = UInt32.MinValue,   ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = UInt32.MaxValue,   ExpectedResult = (long)0x00000000ffffffff, ExpectedHex = "ffffffff"},
      }) {
        val = (UInt48)test.Value;

        Assert.That(val.ToInt64(), Is.EqualTo(test.ExpectedResult), $"value = {val}");
        Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex), $"value = {val}");
      }
    }

    [Test]
    public void TestOpExplicitToInt64()
    {
      long max = (long)UInt48.MaxValue;
      long min = (long)UInt48.MinValue;

      Assert.That((long)0x0000ffffffffffff == max, Is.True);
      Assert.That((long)0x0000000000000000 == min, Is.True);
    }

    [Test]
    public void TestOpExplicitToUInt64()
    {
      ulong max = (ulong)UInt48.MaxValue;
      ulong min = (ulong)UInt48.MinValue;

      Assert.That((ulong)0x0000ffffffffffff == max, Is.True);
      Assert.That((ulong)0x0000000000000000 == min, Is.True);
    }

    [Test]
    public void TestToInt64()
    {
      Assert.That((long)0x0000ffffffffffff == UInt48.MaxValue.ToInt64(), Is.True);
      Assert.That((long)0x0000000000000000 == UInt48.MinValue.ToInt64(), Is.True);

      UInt48 val = (UInt48)0x123456789abc;

      Assert.That((long)0x0000123456789abc == val.ToInt64(), Is.True);
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.That((ulong)0x0000ffffffffffff == UInt48.MaxValue.ToUInt64(), Is.True);
      Assert.That((ulong)0x0000000000000000 == UInt48.MinValue.ToUInt64(), Is.True);

      UInt48 val = (UInt48)0x123456789abc;

      Assert.That((ulong)0x0000123456789abc == val.ToUInt64(), Is.True);
    }

    [Test]
    public void TestIConvertible()
    {
#pragma warning disable CA1305
      Assert.That(Convert.ChangeType((UInt48)1, typeof(bool)), Is.EqualTo(true));
      Assert.That(Convert.ChangeType((UInt48)0, typeof(bool)), Is.EqualTo(false));
      Assert.That(Convert.ChangeType((UInt48)0xff, typeof(byte)), Is.EqualTo((byte)0xff));
      Assert.That(Convert.ChangeType((UInt48)0x7f, typeof(sbyte)), Is.EqualTo((sbyte)0x7f));
      Assert.That(Convert.ChangeType((UInt48)0x7fff, typeof(short)), Is.EqualTo((short)0x7fff));
      Assert.That(Convert.ChangeType((UInt48)0xffff, typeof(ushort)), Is.EqualTo((ushort)0xffff));
      Assert.That(Convert.ChangeType((UInt48)0x7fffffff, typeof(int)), Is.EqualTo((int)0x7fffffff));
      Assert.That(Convert.ChangeType((UInt48)0xffffffff, typeof(uint)), Is.EqualTo((uint)0xffffffff));
      Assert.That(Convert.ChangeType((UInt48)0x0000ffffffffffff, typeof(long)), Is.EqualTo((long)0x0000ffffffffffff));
      Assert.That(Convert.ChangeType((UInt48)0x0000ffffffffffff, typeof(ulong)), Is.EqualTo((ulong)0x0000ffffffffffff));
#pragma warning restore CA1305

      foreach (var t in new[] {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
      }) {
#pragma warning disable CA1305
        Assert.Throws<OverflowException>(() => Convert.ChangeType(UInt48.MaxValue, t), t.FullName ?? t.ToString());
#pragma warning restore CA1305
      }

      foreach (var t in new[] {
        typeof(long),
        typeof(ulong),
      }) {
#pragma warning disable CA1305
        Assert.DoesNotThrow(() => Convert.ChangeType(UInt48.MaxValue, t), t.FullName ?? t.ToString());
#pragma warning restore CA1305
      }
    }

    [Test]
    public void TestIConvertible_ToDateTime()
    {
#pragma warning disable CA1305
      Assert.Throws<InvalidCastException>(() => Convert.ChangeType(UInt24.Zero, typeof(DateTime)));
#pragma warning restore CA1305
      Assert.Throws<InvalidCastException>(() => ((IConvertible)UInt24.Zero).ToDateTime(provider: null));
    }

    [Test]
    public void TestToString()
    {
#pragma warning disable CA1305
      Assert.That(UInt48.Zero.ToString(), Is.EqualTo("0"));
      Assert.That(UInt48.Zero.ToString("D4"), Is.EqualTo("0000"));
      Assert.That(UInt48.MaxValue.ToString(), Is.EqualTo("281474976710655"));
      Assert.That(UInt48.MaxValue.ToString("X"), Is.EqualTo("FFFFFFFFFFFF"));
#pragma warning restore CA1305
    }

#if SYSTEM_ISPANFORMATTABLE
    [Test]
    public void TestTryFormat()
    {
      Span<char> destination = stackalloc char[12];
      int charsWritten = default;

      Assert.That(UInt48.Zero.TryFormat(destination, out charsWritten, ReadOnlySpan<char>.Empty, provider: null), Is.True, "#1");
      Assert.That(charsWritten, Is.EqualTo(1), $"#1 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("0"), $"#1 formatted string");

      Assert.That(UInt48.Zero.TryFormat(destination, out charsWritten, "D4", provider: null), Is.True, "#2");
      Assert.That(charsWritten, Is.EqualTo(4), $"#2 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("0000"), $"#2 formatted string");

      Assert.That(UInt48.MaxValue.TryFormat(destination, out charsWritten, "X", provider: null), Is.True, "#3");
      Assert.That(charsWritten, Is.EqualTo(12), $"#3 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("FFFFFFFFFFFF"), $"#3 formatted string");
    }

    [Test]
    public void TestTryFormat_DestinationTooShort()
    {
      Assert.That(UInt48.Zero.TryFormat(Array.Empty<char>(), out var charsWritten, string.Empty, provider: null), Is.False, "#1");
      Assert.That(UInt48.MaxValue.TryFormat(stackalloc char[UInt48.MaxValue.ToString("D", null).Length - 1], out charsWritten, string.Empty, provider: null), Is.False, "#2");
    }
#endif

    [Test]
    public void TestParse_String()
    {
      Assert.That(UInt48.Parse("0"), Is.EqualTo(UInt48.Zero));
      Assert.That(UInt48.Parse("1"), Is.EqualTo((UInt48)1));
      Assert.That(UInt48.Parse("281474976710655"), Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.Parse("0", provider: null), Is.EqualTo(UInt48.Zero));
      Assert.That(UInt48.Parse("1", provider: null), Is.EqualTo((UInt48)1));
      Assert.That(UInt48.Parse("281474976710655", provider: null), Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.Throws<ArgumentNullException>(() => UInt48.Parse(null!, provider: null));
      Assert.Throws<OverflowException>(() => UInt48.Parse("-1", provider: null));
      Assert.Throws<OverflowException>(() => UInt48.Parse("281474976710656", provider: null));
      Assert.Throws<FormatException>(() => UInt48.Parse("FFFFFFFFFFFF", provider: null));
    }

    [Test]
    public void TestParse_String_WithNumberStyles()
    {
      Assert.That(UInt48.Parse("0", style: NumberStyles.AllowHexSpecifier), Is.EqualTo(UInt48.Zero), "#0");

      Assert.That(UInt48.Parse("456789ABCDEF", style: NumberStyles.AllowHexSpecifier), Is.EqualTo((UInt48)0x456789ABCDEF), "#1");
    }

#if SYSTEM_INT32_PARSE_READONLYSPAN_OF_CHAR
    [Test]
    public void TestParse_ReadOnlySpanOfChar()
    {
      Assert.That(UInt48.Parse("_0".AsSpan(1)), Is.EqualTo(UInt48.Zero));
      Assert.That(UInt48.Parse("_1".AsSpan(1)), Is.EqualTo((UInt48)1));
      Assert.That(UInt48.Parse("_281474976710655".AsSpan(1)), Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.Parse("_0".AsSpan(1), provider: null), Is.EqualTo(UInt48.Zero));
      Assert.That(UInt48.Parse("_1".AsSpan(1), provider: null), Is.EqualTo((UInt48)1));
      Assert.That(UInt48.Parse("_281474976710655".AsSpan(1), provider: null), Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.Throws<OverflowException>(() => UInt48.Parse("_-1".AsSpan(1), provider: null));
      Assert.Throws<OverflowException>(() => UInt48.Parse("_281474976710656".AsSpan(1), provider: null));
      Assert.Throws<FormatException>(() => UInt48.Parse("_FFFFFFFFFFFF".AsSpan(1), provider: null));
    }

    [Test]
    public void TestParse_ReadOnlySpanOfChar_WithNumberStyles()
    {
      Assert.That(UInt48.Parse("_0".AsSpan(1), style: NumberStyles.AllowHexSpecifier), Is.EqualTo(UInt48.Zero), "#0");

      Assert.That(UInt48.Parse("_456789ABCDEF".AsSpan(1), style: NumberStyles.AllowHexSpecifier), Is.EqualTo((UInt48)0x456789ABCDEF), "#1");
    }
#endif

    [Test]
    public void TestTryParse_String()
    {
      Assert.That(UInt48.TryParse("0", out var result0), Is.True);
      Assert.That(result0, Is.EqualTo(UInt48.Zero));

      Assert.That(UInt48.TryParse("1", out var result1), Is.True);
      Assert.That(result1, Is.EqualTo((UInt48)1));

      Assert.That(UInt48.TryParse("281474976710655", out var result2), Is.True);
      Assert.That(result2, Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.TryParse("0", provider: null, out var result3), Is.True);
      Assert.That(result3, Is.EqualTo(UInt48.Zero));

      Assert.That(UInt48.TryParse("1", provider: null, out var result4), Is.True);
      Assert.That(result4, Is.EqualTo((UInt48)1));

      Assert.That(UInt48.TryParse("281474976710655", provider: null, out var result5), Is.True);
      Assert.That(result5, Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.TryParse(null, provider: null, out _), Is.False);
      Assert.That(UInt48.TryParse("-1", provider: null, out _), Is.False); // overflow
      Assert.That(UInt48.TryParse("281474976710656", provider: null, out _), Is.False); // overflow
      Assert.That(UInt48.TryParse("FFFFFFFFFFFF", provider: null, out _), Is.False); // invalid format
    }

    [Test]
    public void TestTryParse_String_WithNumberStyles()
    {
      Assert.That(UInt48.TryParse("0", style: NumberStyles.AllowHexSpecifier, provider: null, out var result0), Is.True, "#0");
      Assert.That(result0, Is.EqualTo(UInt48.Zero), "#0");

      Assert.That(UInt48.TryParse("456789ABCDEF", style: NumberStyles.AllowHexSpecifier, provider: null, out var result1), Is.True, "#1");
      Assert.That(result1, Is.EqualTo((UInt48)0x456789ABCDEF), "#1");

      Assert.That(UInt48.TryParse("X", style: NumberStyles.AllowHexSpecifier, provider: null, out _), Is.False, "#2");
    }

#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
    [Test]
    public void TestTryParse_ReadOnlySpanOfChar()
    {
      Assert.That(UInt48.TryParse("_0".AsSpan(1), out var result0), Is.True);
      Assert.That(result0, Is.EqualTo(UInt48.Zero));

      Assert.That(UInt48.TryParse("_1".AsSpan(1), out var result1), Is.True);
      Assert.That(result1, Is.EqualTo((UInt48)1));

      Assert.That(UInt48.TryParse("_281474976710655".AsSpan(1), out var result2), Is.True);
      Assert.That(result2, Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.TryParse("_0".AsSpan(1), provider: null, out var result3), Is.True);
      Assert.That(result3, Is.EqualTo(UInt48.Zero));

      Assert.That(UInt48.TryParse("_1".AsSpan(1), provider: null, out var result4), Is.True);
      Assert.That(result4, Is.EqualTo((UInt48)1));

      Assert.That(UInt48.TryParse("_281474976710655".AsSpan(1), provider: null, out var result5), Is.True);
      Assert.That(result5, Is.EqualTo((UInt48)0xFFFFFFFFFFFF));

      Assert.That(UInt48.TryParse("_-1".AsSpan(1), provider: null, out _), Is.False); // overflow
      Assert.That(UInt48.TryParse("_281474976710656".AsSpan(1), provider: null, out _), Is.False); // overflow
      Assert.That(UInt48.TryParse("_FFFFFFFFFFFF".AsSpan(1), provider: null, out _), Is.False); // invalid format
    }

    [Test]
    public void TestTryParse_ReadOnlySpanOfChar_WithNumberStyles()
    {
      Assert.That(UInt48.TryParse("_0".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out var result0), Is.True, "#0");
      Assert.That(result0, Is.EqualTo(UInt48.Zero), "#0");

      Assert.That(UInt48.TryParse("_456789ABCDEF".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out var result1), Is.True, "#1");
      Assert.That(result1, Is.EqualTo((UInt48)0x456789ABCDEF), "#1");

      Assert.That(UInt48.TryParse("_X".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out _), Is.False, "#2");
    }
#endif
  }
}
