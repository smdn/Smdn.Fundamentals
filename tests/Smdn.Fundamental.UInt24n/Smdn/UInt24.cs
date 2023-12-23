// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public partial class UInt24Tests {
    [Test]
    public void TestSizeOfStructure()
    {
      Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(UInt24)), Is.EqualTo(3));
    }

    [Test]
    public void TestConstruct()
    {
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, isBigEndian: true), Is.EqualTo((UInt24)0xff0000));
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00 }, isBigEndian: true), Is.EqualTo((UInt24)0xff0000));
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, isBigEndian: true), Is.EqualTo((UInt24)0x0000cc));
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, isBigEndian: false), Is.EqualTo((UInt24)0x0000ff));
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00 }, isBigEndian: false), Is.EqualTo((UInt24)0x0000ff));
      Assert.That(new UInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, isBigEndian: false), Is.EqualTo((UInt24)0xcc0000));

      Assert.Throws<ArgumentNullException>(() => new UInt24(null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => new UInt24(new byte[] { 0x00, 0x00 }, -1));
      Assert.Throws<ArgumentException>(() => new UInt24(new byte[] { 0x00, 0x00 }, 0));
      Assert.Throws<ArgumentException>(() => new UInt24(new byte[] { 0x00, 0x00, 0x00 }, 1));
    }

    [Test]
    public void TestConstruct_ReadOnlySpan()
    {
      Assert.That(
        new UInt24(stackalloc byte[] { 0x01, 0x23, 0x45 }, isBigEndian: true),
        Is.EqualTo((UInt24)0x012345)
      );
      Assert.That(
        new UInt24(stackalloc byte[] { 0x01, 0x23, 0x45 }, isBigEndian: false),
        Is.EqualTo((UInt24)0x452301)
      );

      Assert.Throws<ArgumentException>(() => new UInt24(ReadOnlySpan<byte>.Empty));
      Assert.Throws<ArgumentException>(() => new UInt24(stackalloc byte[] { 0x00 }));
      Assert.Throws<ArgumentException>(() => new UInt24(stackalloc byte[] { 0x00, 0x00 }));
    }

    [Test]
    public void Test()
    {
      Assert.That((UInt24)0xff00ff, Is.EqualTo((UInt24)(int)0xff00ff));

      var val = (UInt24)(int)0x123456;

      Assert.That(val.ToInt32(), Is.EqualTo((int)0x123456));

      var zero = (UInt24)0x000000;
      var one  = (UInt24)0x000001;

      Assert.That(zero.Equals(zero), Is.True);
      Assert.That(zero.Equals(one), Is.False);
      Assert.That(zero.Equals(null), Is.False);
    }

    [Test]
    public void TestConstants()
    {
      Assert.That(UInt24.Zero, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)), nameof(UInt24.Zero));
      Assert.That(UInt24.One, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x01 }, isBigEndian: true)), nameof(UInt24.One));
      Assert.That(UInt24.MinValue, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)), nameof(UInt24.MinValue));
      Assert.That(UInt24.MaxValue, Is.EqualTo(new UInt24(stackalloc byte[3] { 0xff, 0xff, 0xff }, isBigEndian: true)), nameof(UInt24.MaxValue));
    }

    [Test]
    public void TestEquals()
    {
      Assert.That(UInt24.Zero.Equals(UInt24.Zero), Is.True);
      Assert.That(UInt24.Zero.Equals(0), Is.True);
      Assert.That(UInt24.Zero.Equals(0u), Is.True);
      Assert.That(UInt24.One.Equals(UInt24.Zero), Is.False);
      Assert.That(UInt24.One.Equals(0), Is.False);
      Assert.That(UInt24.One.Equals(0u), Is.False);
      Assert.That(UInt24.Zero.Equals(UInt24.MaxValue), Is.False);
      Assert.That(UInt24.Zero.Equals(int.MaxValue), Is.False);
      Assert.That(UInt24.Zero.Equals(uint.MaxValue), Is.False);

      object val;

      val = UInt24.Zero; Assert.That(UInt24.Zero.Equals(val), Is.True);
      val = null; Assert.That(UInt24.Zero.Equals(val), Is.False);
    }

    [TestCase(0, true)]
    [TestCase(0u, true)]
    [TestCase((object)null, false)]
    [TestCase(true, false)]
    public void TestEquals_Object(object value, bool expected)
      => Assert.That(UInt24.Zero.Equals(value), Is.EqualTo(expected));


    [Test]
    public void TestCompareTo()
    {
      var zero = (UInt24)0x000000;
      var one  = (UInt24)0x000001;

      Assert.That(zero.CompareTo(null), Is.EqualTo(1), "CompareTo(null)");

      Assert.That(0 == zero.CompareTo(zero), "Zero.CompareTo(Zero)");
      Assert.That(0 > zero.CompareTo(one), "Zero.CompareTo(Zero)");
      Assert.That(0 < one.CompareTo(zero), "One.CompareTo(Zero)");
      Assert.That(0 == one.CompareTo(one), "One.CompareTo(One)");

      Assert.That(0 < one.CompareTo(0), "One.CompareTo(0)");
      Assert.That(0 < one.CompareTo(0u), "One.CompareTo(0u)");
    }

    [TestCase(false)]
    [TestCase(0.0)]
    [TestCase((byte)0)]
    [TestCase("0")]
    public void TestCompareTo_NotComparable(object value)
      => Assert.Throws<ArgumentException>(() => UInt24.Zero.CompareTo(value));

    [Test]
    public void TestOpExplicitFromInt16()
    {
      foreach (var test in new[] {
        new {Value = (short)0x000000, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (short)0x007fff, ExpectedResult = 0x00007fff, ExpectedHex = "7fff"},
        new {Value = Int16.MaxValue,  ExpectedResult = 0x00007fff, ExpectedHex = "7fff"},
        new {Value = (short)-1,       ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = Int16.MinValue,  ExpectedResult = 0x00ff8000, ExpectedHex = "ff8000"},
      }) {

        try {
          UInt24 val = unchecked((UInt24)test.Value);

          Assert.That(val.ToInt32(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
          Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedFromInt16()
    {
      foreach (var test in new[] {
        new {Value = (short)-1},
        new {Value = Int16.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = checked((UInt24)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromInt32()
    {
      foreach (var test in new[] {
        new {Value = (int)0x00000000, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (int)0x00ffffff, ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = (int)-1        , ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = (int)0x01000000, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = Int32.MaxValue,  ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = Int32.MinValue,  ExpectedResult = 0x00000000, ExpectedHex = "0"},
      }) {

        try {
          UInt24 val = unchecked((UInt24)test.Value);

          Assert.That(val.ToInt32(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
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
        new {Value = (int)0x01000000},
        new {Value = Int32.MaxValue},
        new {Value = Int32.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = checked((UInt24)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt32()
    {
      foreach (var test in new[] {
        new {Value = (uint)0x00000000,  ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (uint)0x00ffffff,  ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = UInt32.MinValue,   ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (uint)0x01000000,  ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (uint)0xffffffff,  ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = UInt32.MaxValue,   ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
      }) {

        try {
          UInt24 val = unchecked((UInt24)test.Value);

          Assert.That(val.ToInt32(), Is.EqualTo(test.ExpectedResult), $"value = {test.ExpectedHex}");
          Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedFromUInt32()
    {
      foreach (var test in new[] {
        new {Value = (uint)0x01000000},
        new {Value = (uint)0xffffffff},
        new {Value = UInt32.MaxValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = checked((UInt24)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitToInt16()
    {
      foreach (var test in new[] {
        new {Value = UInt24.MinValue,   ExpectedResult = (short)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x000000,  ExpectedResult = (short)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x007fff,  ExpectedResult = (short)0x7fff, ExpectedHex = "7fff"},
        new {Value = (UInt24)0x008000,  ExpectedResult = unchecked((short)(ushort)0x8000), ExpectedHex = "8000"},
        new {Value = (UInt24)0xffffff,  ExpectedResult = unchecked((short)(ushort)0xffff), ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedResult = unchecked((short)(ushort)0xffff), ExpectedHex = "ffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        try {
          Assert.That(unchecked((short)test.Value), Is.EqualTo(test.ExpectedResult));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedToInt16()
    {
      foreach (var test in new[] {
        new {Value = (UInt24)0x008000,  ExpectedHex = "8000"},
        new {Value = (UInt24)0xffffff,  ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedHex = "ffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        Assert.Throws<OverflowException>(() => { var s = checked((short)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitToUInt16()
    {
      foreach (var test in new[] {
        new {Value = UInt24.MinValue,   ExpectedResult = (ushort)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x000000,  ExpectedResult = (ushort)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x007fff,  ExpectedResult = (ushort)0x7fff, ExpectedHex = "7fff"},
        new {Value = (UInt24)0x008000,  ExpectedResult = (ushort)0x8000, ExpectedHex = "8000"},
        new {Value = (UInt24)0x00ffff,  ExpectedResult = (ushort)0xffff, ExpectedHex = "ffff"},
        new {Value = (UInt24)0x010000,  ExpectedResult = (ushort)0x0000, ExpectedHex = "10000"},
        new {Value = (UInt24)0xffffff,  ExpectedResult = (ushort)0xffff, ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedResult = (ushort)0xffff, ExpectedHex = "ffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        try {
          Assert.That(unchecked((ushort)test.Value), Is.EqualTo(test.ExpectedResult));
        }
        catch (OverflowException) {
          Assert.Fail($"OverflowException thrown: value = {test.ExpectedHex}");
        }
      }
    }

    [Test]
    public void TestOpExplicitCheckedToUInt16()
    {
      foreach (var test in new[] {
        new {Value = (UInt24)0x010000,  ExpectedHex = "10000"},
        new {Value = (UInt24)0xffffff,  ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedHex = "ffffff"},
      }) {
        Assert.That(test.Value.ToString("x", null), Is.EqualTo(test.ExpectedHex));

        Assert.Throws<OverflowException>(() => { var us = checked((ushort)test.Value); });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt16()
    {
      UInt24 val;

      foreach (var test in new[] {
        new {Value = (ushort)0,       ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (ushort)0x0000,  ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (ushort)0xffff,  ExpectedResult = 0x0000ffff, ExpectedHex = "ffff"},
        new {Value = UInt16.MinValue, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = UInt16.MaxValue, ExpectedResult = 0x0000ffff, ExpectedHex = "ffff"},
      }) {
        val = (UInt24)test.Value;

        Assert.That(val.ToInt32(), Is.EqualTo(test.ExpectedResult), $"value = {val}");
        Assert.That(val.ToString("x", null), Is.EqualTo(test.ExpectedHex), $"value = {val}");
      }
    }

    [Test]
    public void TestOpExplicitToInt32()
    {
      int max = (int)UInt24.MaxValue;
      int min = (int)UInt24.MinValue;

      Assert.That((int)0x00ffffff == max, Is.True);
      Assert.That((int)0x00000000 == min, Is.True);
    }

    [Test]
    public void TestOpExplicitToUInt32()
    {
      uint max = (uint)UInt24.MaxValue;
      uint min = (uint)UInt24.MinValue;

      Assert.That((uint)0x00ffffff == max, Is.True);
      Assert.That((uint)0x00000000 == min, Is.True);
    }

    [Test]
    public void TestToInt32()
    {
      Assert.That((int)0x00ffffff == UInt24.MaxValue.ToInt32(), Is.True);
      Assert.That((int)0x00000000 == UInt24.MinValue.ToInt32(), Is.True);

      UInt24 val = (UInt24)0x123456;

      Assert.That((int)0x00123456 == val.ToInt32(), Is.True);
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.That((uint)0x00ffffff == UInt24.MaxValue.ToUInt32(), Is.True);
      Assert.That((uint)0x00000000 == UInt24.MinValue.ToUInt32(), Is.True);

      UInt24 val = (UInt24)0x123456;

      Assert.That((uint)0x00123456 == val.ToUInt32(), Is.True);
    }

    [Test]
    public void TestIConvertible()
    {
#pragma warning disable CA1305
      Assert.That(Convert.ChangeType((UInt24)1, typeof(bool)), Is.EqualTo(true));
      Assert.That(Convert.ChangeType((UInt24)0, typeof(bool)), Is.EqualTo(false));
      Assert.That(Convert.ChangeType((UInt24)0xff, typeof(byte)), Is.EqualTo((byte)0xff));
      Assert.That(Convert.ChangeType((UInt24)0x7f, typeof(sbyte)), Is.EqualTo((sbyte)0x7f));
      Assert.That(Convert.ChangeType((UInt24)0x7fff, typeof(short)), Is.EqualTo((short)0x7fff));
      Assert.That(Convert.ChangeType((UInt24)0xffff, typeof(ushort)), Is.EqualTo((ushort)0xffff));
      Assert.That(Convert.ChangeType((UInt24)0xffffff, typeof(int)), Is.EqualTo((int)0x00ffffff));
      Assert.That(Convert.ChangeType((UInt24)0xffffff, typeof(uint)), Is.EqualTo((uint)0x00ffffff));
      Assert.That(Convert.ChangeType((UInt24)0xffffff, typeof(long)), Is.EqualTo((long)0x0000000000ffffff));
      Assert.That(Convert.ChangeType((UInt24)0xffffff, typeof(ulong)), Is.EqualTo((ulong)0x0000000000ffffff));
#pragma warning restore CA1305

      foreach (var t in new[] {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
      }) {
#pragma warning disable CA1305
        Assert.Throws<OverflowException>(() => Convert.ChangeType(UInt24.MaxValue, t), t.FullName ?? t.ToString());
#pragma warning restore CA1305
      }

      foreach (var t in new[] {
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
      }) {
#pragma warning disable CA1305
        Assert.DoesNotThrow(() => Convert.ChangeType(UInt24.MaxValue, t), t.FullName ?? t.ToString());
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
      Assert.That(UInt24.Zero.ToString(), Is.EqualTo("0"));
      Assert.That(UInt24.Zero.ToString("D4"), Is.EqualTo("0000"));
      Assert.That(UInt24.MaxValue.ToString(), Is.EqualTo("16777215"));
      Assert.That(UInt24.MaxValue.ToString("X"), Is.EqualTo("FFFFFF"));
#pragma warning restore CA1305
    }

#if SYSTEM_ISPANFORMATTABLE
    [Test]
    public void TestTryFormat()
    {
      Span<char> destination = stackalloc char[6];
      int charsWritten = default;

      Assert.That(UInt24.Zero.TryFormat(destination, out charsWritten, ReadOnlySpan<char>.Empty, provider: null), Is.True, "#1");
      Assert.That(charsWritten, Is.EqualTo(1), $"#1 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("0"), $"#1 formatted string");

      Assert.That(UInt24.Zero.TryFormat(destination, out charsWritten, "D4", provider: null), Is.True, "#2");
      Assert.That(charsWritten, Is.EqualTo(4), $"#2 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("0000"), $"#2 formatted string");

      Assert.That(UInt24.MaxValue.TryFormat(destination, out charsWritten, "X", provider: null), Is.True, "#3");
      Assert.That(charsWritten, Is.EqualTo(6), $"#3 {nameof(charsWritten)}");
      Assert.That(new string(destination.Slice(0, charsWritten)), Is.EqualTo("FFFFFF"), $"#3 formatted string");
    }

    [Test]
    public void TestTryFormat_DestinationTooShort()
    {
      Assert.That(UInt24.Zero.TryFormat(Array.Empty<char>(), out var charsWritten, string.Empty, provider: null), Is.False, "#1");
      Assert.That(UInt24.MaxValue.TryFormat(stackalloc char[UInt24.MaxValue.ToString("D", null).Length - 1], out charsWritten, string.Empty, provider: null), Is.False, "#2");
    }
#endif

    [Test]
    public void TestParse_String()
    {
      Assert.That(UInt24.Parse("0"), Is.EqualTo(UInt24.Zero));
      Assert.That(UInt24.Parse("1"), Is.EqualTo((UInt24)1));
      Assert.That(UInt24.Parse("16777215"), Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.Parse("0", provider: null), Is.EqualTo(UInt24.Zero));
      Assert.That(UInt24.Parse("1", provider: null), Is.EqualTo((UInt24)1));
      Assert.That(UInt24.Parse("16777215", provider: null), Is.EqualTo((UInt24)0xFFFFFF));

      Assert.Throws<ArgumentNullException>(() => UInt24.Parse(null!, provider: null));
      Assert.Throws<OverflowException>(() => UInt24.Parse("-1", provider: null));
      Assert.Throws<OverflowException>(() => UInt24.Parse("16777216", provider: null));
      Assert.Throws<FormatException>(() => UInt24.Parse("FFFFFF", provider: null));
    }

    [Test]
    public void TestParse_String_WithNumberStyles()
    {
      Assert.That(UInt24.Parse("0", style: NumberStyles.AllowHexSpecifier), Is.EqualTo(UInt24.Zero), "#0");

      Assert.That(UInt24.Parse("ABCDEF", style: NumberStyles.AllowHexSpecifier), Is.EqualTo((UInt24)0xABCDEF), "#1");
    }

#if SYSTEM_INT32_PARSE_READONLYSPAN_OF_CHAR
    [Test]
    public void TestParse_ReadOnlySpanOfChar()
    {
      Assert.That(UInt24.Parse("_0".AsSpan(1)), Is.EqualTo(UInt24.Zero));
      Assert.That(UInt24.Parse("_1".AsSpan(1)), Is.EqualTo((UInt24)1));
      Assert.That(UInt24.Parse("_16777215".AsSpan(1)), Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.Parse("_0".AsSpan(1), provider: null), Is.EqualTo(UInt24.Zero));
      Assert.That(UInt24.Parse("_1".AsSpan(1), provider: null), Is.EqualTo((UInt24)1));
      Assert.That(UInt24.Parse("_16777215".AsSpan(1), provider: null), Is.EqualTo((UInt24)0xFFFFFF));

      Assert.Throws<OverflowException>(() => UInt24.Parse("_-1".AsSpan(1), provider: null));
      Assert.Throws<OverflowException>(() => UInt24.Parse("_16777216".AsSpan(1), provider: null));
      Assert.Throws<FormatException>(() => UInt24.Parse("_FFFFFF".AsSpan(1), provider: null));
    }

    [Test]
    public void TestParse_ReadOnlySpanOfChar_WithNumberStyles()
    {
      Assert.That(UInt24.Parse("_0".AsSpan(1), style: NumberStyles.AllowHexSpecifier), Is.EqualTo(UInt24.Zero), "#0");

      Assert.That(UInt24.Parse("_ABCDEF".AsSpan(1), style: NumberStyles.AllowHexSpecifier), Is.EqualTo((UInt24)0xABCDEF), "#1");
    }
#endif

    [Test]
    public void TestTryParse_String()
    {
      Assert.That(UInt24.TryParse("0", out var result0), Is.True);
      Assert.That(result0, Is.EqualTo(UInt24.Zero));

      Assert.That(UInt24.TryParse("1", out var result1), Is.True);
      Assert.That(result1, Is.EqualTo((UInt24)1));

      Assert.That(UInt24.TryParse("16777215", out var result2), Is.True);
      Assert.That(result2, Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.TryParse("0", provider: null, out var result3), Is.True);
      Assert.That(result3, Is.EqualTo(UInt24.Zero));

      Assert.That(UInt24.TryParse("1", provider: null, out var result4), Is.True);
      Assert.That(result4, Is.EqualTo((UInt24)1));

      Assert.That(UInt24.TryParse("16777215", provider: null, out var result5), Is.True);
      Assert.That(result5, Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.TryParse(null, provider: null, out _), Is.False);
      Assert.That(UInt24.TryParse("-1", provider: null, out _), Is.False); // overflow
      Assert.That(UInt24.TryParse("16777216", provider: null, out _), Is.False); // overflow
      Assert.That(UInt24.TryParse("FFFFFF", provider: null, out _), Is.False); // invalid format
    }

    [Test]
    public void TestTryParse_String_WithNumberStyles()
    {
      Assert.That(UInt24.TryParse("0", style: NumberStyles.AllowHexSpecifier, provider: null, out var result0), Is.True, "#0");
      Assert.That(result0, Is.EqualTo(UInt24.Zero), "#0");

      Assert.That(UInt24.TryParse("ABCDEF", style: NumberStyles.AllowHexSpecifier, provider: null, out var result1), Is.True, "#1");
      Assert.That(result1, Is.EqualTo((UInt24)0xABCDEF), "#1");

      Assert.That(UInt24.TryParse("X", style: NumberStyles.AllowHexSpecifier, provider: null, out _), Is.False, "#2");
    }

#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
    [Test]
    public void TestTryParse_ReadOnlySpanOfChar()
    {
      Assert.That(UInt24.TryParse("_0".AsSpan(1), out var result0), Is.True);
      Assert.That(result0, Is.EqualTo(UInt24.Zero));

      Assert.That(UInt24.TryParse("_1".AsSpan(1), out var result1), Is.True);
      Assert.That(result1, Is.EqualTo((UInt24)1));

      Assert.That(UInt24.TryParse("_16777215".AsSpan(1), out var result2), Is.True);
      Assert.That(result2, Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.TryParse("_0".AsSpan(1), provider: null, out var result3), Is.True);
      Assert.That(result3, Is.EqualTo(UInt24.Zero));

      Assert.That(UInt24.TryParse("_1".AsSpan(1), provider: null, out var result4), Is.True);
      Assert.That(result4, Is.EqualTo((UInt24)1));

      Assert.That(UInt24.TryParse("_16777215".AsSpan(1), provider: null, out var result5), Is.True);
      Assert.That(result5, Is.EqualTo((UInt24)0xFFFFFF));

      Assert.That(UInt24.TryParse("_-1".AsSpan(1), provider: null, out _), Is.False); // overflow
      Assert.That(UInt24.TryParse("_16777216".AsSpan(1), provider: null, out _), Is.False); // overflow
      Assert.That(UInt24.TryParse("_FFFFFF".AsSpan(1), provider: null, out _), Is.False); // invalid format
    }

    [Test]
    public void TestTryParse_ReadOnlySpanOfChar_WithNumberStyles()
    {
      Assert.That(UInt24.TryParse("_0".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out var result0), Is.True, "#0");
      Assert.That(result0, Is.EqualTo(UInt24.Zero), "#0");

      Assert.That(UInt24.TryParse("_ABCDEF".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out var result1), Is.True, "#1");
      Assert.That(result1, Is.EqualTo((UInt24)0xABCDEF), "#1");

      Assert.That(UInt24.TryParse("_X".AsSpan(1), style: NumberStyles.AllowHexSpecifier, provider: null, out _), Is.False, "#2");
    }
#endif
  }
}
