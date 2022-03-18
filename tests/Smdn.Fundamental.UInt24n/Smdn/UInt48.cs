// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET6_0_OR_GREATER
#define SYSTEM_ISPANFORMATTABLE
#endif

using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UInt48Tests {
    [Test]
    public void TestSizeOfStructure()
    {
      Assert.AreEqual(6, System.Runtime.InteropServices.Marshal.SizeOf(typeof(UInt48)));
    }

    [Test]
    public void TestConstruct()
    {
      Assert.AreEqual((UInt48)0xff0000000000,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, isBigEndian: true));
      Assert.AreEqual((UInt48)0xff0000000000,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true));
      Assert.AreEqual((UInt48)0x0000000000cc,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, isBigEndian: true));
      Assert.AreEqual((UInt48)0x0000000000ff,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, isBigEndian: false));
      Assert.AreEqual((UInt48)0x0000000000ff,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: false));
      Assert.AreEqual((UInt48)0xcc0000000000,
                      new UInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, isBigEndian: false));

      Assert.Throws<ArgumentNullException>(() => new UInt48(null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, -1));
      Assert.Throws<ArgumentException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 0));
      Assert.Throws<ArgumentException>(() => new UInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 1));
    }

    [Test]
    public void TestConstruct_ReadOnlySpan()
    {
      Assert.AreEqual(
        (UInt48)0x0123456789AB,
        new UInt48(stackalloc byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, isBigEndian: true)
      );
      Assert.AreEqual(
        (UInt48)0xAB8967452301,
        new UInt48(stackalloc byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB }, isBigEndian: false)
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
      Assert.AreEqual((UInt48)(int)0x00ff00ff, (UInt48)(long)0x00ff00ff);

      var val = (UInt48)(long)0x123456789012;

      Assert.AreEqual((long)0x123456789012, val.ToInt64());

      var zero = (UInt48)0x000000000000;
      var one  = (UInt48)0x000000000001;

      Assert.IsTrue(zero.Equals(zero));
      Assert.IsFalse(zero.Equals(one));
      Assert.IsFalse(zero.Equals(null));

      Assert.AreEqual(1, zero.CompareTo(null));

      Assert.That(0 < one.CompareTo(zero));
      Assert.That(0 < one.CompareTo(0L));
      Assert.That(0 < one.CompareTo(0UL));
    }

    [Test]
    public void TestOpLessThan()
    {
      Assert.IsFalse(UInt48.Zero < (UInt48)0, "#1");
      Assert.IsTrue(UInt48.Zero < (UInt48)1, "#2");
      Assert.IsFalse((UInt48)1 < UInt48.Zero, "#3");

      Assert.IsFalse((UInt48)0xffffffffffff < UInt48.MaxValue, "#4");
      Assert.IsFalse(UInt48.MaxValue < (UInt48)0xfffffffffffe, "#5");
      Assert.IsTrue((UInt48)0xfffffffffffe < UInt48.MaxValue, "#6");
    }

    [Test]
    public void TestOpLessThanOrEqual()
    {
      Assert.IsTrue(UInt48.Zero <= (UInt48)0, "#1");
      Assert.IsTrue(UInt48.Zero <= (UInt48)1, "#2");
      Assert.IsFalse((UInt48)1 <= UInt48.Zero, "#3");

      Assert.IsTrue((UInt48)0xffffffffffff <= UInt48.MaxValue, "#4");
      Assert.IsFalse(UInt48.MaxValue <= (UInt48)0xfffffffffffe, "#5");
      Assert.IsTrue((UInt48)0xfffffffffffe <= UInt48.MaxValue, "#6");
    }

    [Test]
    public void TestOpGreaterThan()
    {
      Assert.IsFalse(UInt48.Zero > (UInt48)0, "#1");
      Assert.IsFalse(UInt48.Zero > (UInt48)1, "#2");
      Assert.IsTrue((UInt48)1 > UInt48.Zero, "#3");

      Assert.IsFalse((UInt48)0xffffffffffff > UInt48.MaxValue, "#4");
      Assert.IsTrue(UInt48.MaxValue > (UInt48)0xfffffffffffe, "#5");
      Assert.IsFalse((UInt48)0xfffffffffffe > UInt48.MaxValue, "#6");
    }

    [Test]
    public void TestOpGreaterThanOrEqual()
    {
      Assert.IsTrue(UInt48.Zero >= (UInt48)0, "#1");
      Assert.IsFalse(UInt48.Zero >= (UInt48)1, "#2");
      Assert.IsTrue((UInt48)1 >= UInt48.Zero, "#3");

      Assert.IsTrue((UInt48)0xffffffffffff >= UInt48.MaxValue, "#4");
      Assert.IsTrue(UInt48.MaxValue >= (UInt48)0xfffffffffffe, "#5");
      Assert.IsFalse((UInt48)0xfffffffffffe >= UInt48.MaxValue, "#6");
    }

    [Test]
    public void TestEquals()
    {
      Assert.IsTrue(UInt48.Zero.Equals(UInt48.Zero));
      Assert.IsTrue(UInt48.Zero.Equals(0L));
      Assert.IsTrue(UInt48.Zero.Equals(0UL));
      Assert.IsFalse(UInt48.Zero.Equals(UInt48.MaxValue));
      Assert.IsFalse(UInt48.Zero.Equals(long.MaxValue));
      Assert.IsFalse(UInt48.Zero.Equals(ulong.MaxValue));

      object val;

      val = UInt48.Zero;
      Assert.IsTrue(UInt48.Zero.Equals(val));

      val = 0L;
      Assert.IsTrue(UInt48.Zero.Equals(val));

      val = 0UL;
      Assert.IsTrue(UInt48.Zero.Equals(val));
    }

    [Test]
    public void TestOpEquality()
    {
      Assert.IsTrue(UInt48.Zero == (UInt48)0);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x000000000010);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x000000001000);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x000000100000);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x000010000000);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x001000000000);
      Assert.IsFalse(UInt48.Zero == (UInt48)0x100000000000);
    }

    [Test]
    public void TestOpIneqality()
    {
      Assert.IsFalse(UInt48.Zero != (UInt48)0);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x000000000010);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x000000001000);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x000000100000);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x000010000000);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x001000000000);
      Assert.IsTrue(UInt48.Zero != (UInt48)0x100000000000);
    }

    [Test]
    public void TestOpExplicitFromInt32()
    {
      foreach (var test in new[] {
        new {Value = (int)0x00000000, ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (int)0x7fffffff, ExpectedResult = (long)0x000000007fffffff, ExpectedHex = "7fffffff"},
        new {Value = Int32.MaxValue,  ExpectedResult = (long)0x000000007fffffff, ExpectedHex = "7fffffff"},
      }) {

        try {
          UInt48 val = (UInt48)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt64(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (int)-1},
        new {Value = Int32.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = (UInt48)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitFromInt64()
    {
      foreach (var test in new[] {
        new {Value = (long)0x0000000000000000, ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (long)0x0000ffffffffffff, ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
      }) {

        try {
          UInt48 val = (UInt48)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt64(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (long)-1},
        new {Value = (long)0x0001000000000000},
        new {Value = Int64.MaxValue},
        new {Value = Int64.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = (UInt48)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt64()
    {
      foreach (var test in new[] {
        new {Value = (ulong)0x0000000000000000,   ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
        new {Value = (ulong)0x0000ffffffffffff,   ExpectedResult = (long)0x0000ffffffffffff, ExpectedHex = "ffffffffffff"},
        new {Value = UInt64.MinValue,             ExpectedResult = (long)0x0000000000000000, ExpectedHex = "0"},
      }) {

        try {
          UInt48 val = (UInt48)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt64(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (ulong)0x0001000000000000},
        new {Value = (ulong)0xffffffffffffffff},
        new {Value = UInt64.MaxValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt48 val = (UInt48)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitToInt32()
    {
      foreach (var test in new[] {
        new {Value = UInt48.MinValue,         ExpectedResult = (int)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x000000000000,  ExpectedResult = (int)0x00000000, ExpectedHex = "0"},
        new {Value = (UInt48)0x00007fffffff,  ExpectedResult = (int)0x7fffffff, ExpectedHex = "7fffffff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        try {
          Assert.IsTrue(test.ExpectedResult == (int)test.Value);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (UInt48)0x000080000000,  ExpectedHex = "80000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedHex = "ffffffffffff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        Assert.Throws<OverflowException>(() => { var i = (int)test.Value; });
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
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        try {
          Assert.IsTrue(test.ExpectedResult == (uint)test.Value);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (UInt48)0x000100000000,  ExpectedHex = "100000000"},
        new {Value = (UInt48)0xffffffffffff,  ExpectedHex = "ffffffffffff"},
        new {Value = UInt48.MaxValue,         ExpectedHex = "ffffffffffff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        Assert.Throws<OverflowException>(() => { var ui = (uint)test.Value; });
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

        Assert.IsTrue(test.ExpectedResult == val.ToInt64(), "value = {0}", val);
        Assert.AreEqual(test.ExpectedHex, val.ToString("x"), "value = {0}", val);
      }
    }

    [Test]
    public void TestOpExplicitToInt64()
    {
      long max = (long)UInt48.MaxValue;
      long min = (long)UInt48.MinValue;

      Assert.IsTrue((long)0x0000ffffffffffff == max);
      Assert.IsTrue((long)0x0000000000000000 == min);
    }

    [Test]
    public void TestOpExplicitToUInt64()
    {
      ulong max = (ulong)UInt48.MaxValue;
      ulong min = (ulong)UInt48.MinValue;

      Assert.IsTrue((ulong)0x0000ffffffffffff == max);
      Assert.IsTrue((ulong)0x0000000000000000 == min);
    }

    [Test]
    public void TestToInt64()
    {
      Assert.IsTrue((long)0x0000ffffffffffff == UInt48.MaxValue.ToInt64());
      Assert.IsTrue((long)0x0000000000000000 == UInt48.MinValue.ToInt64());

      UInt48 val = (UInt48)0x123456789abc;

      Assert.IsTrue((long)0x0000123456789abc == val.ToInt64());
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.IsTrue((ulong)0x0000ffffffffffff == UInt48.MaxValue.ToUInt64());
      Assert.IsTrue((ulong)0x0000000000000000 == UInt48.MinValue.ToUInt64());

      UInt48 val = (UInt48)0x123456789abc;

      Assert.IsTrue((ulong)0x0000123456789abc == val.ToUInt64());
    }

    [Test]
    public void TestIConvertible()
    {
      Assert.AreEqual(true, Convert.ChangeType((UInt48)1, typeof(bool)));
      Assert.AreEqual(false, Convert.ChangeType((UInt48)0, typeof(bool)));
      Assert.AreEqual((byte)0xff, Convert.ChangeType((UInt48)0xff, typeof(byte)));
      Assert.AreEqual((sbyte)0x7f, Convert.ChangeType((UInt48)0x7f, typeof(sbyte)));
      Assert.AreEqual((short)0x7fff, Convert.ChangeType((UInt48)0x7fff, typeof(short)));
      Assert.AreEqual((ushort)0xffff, Convert.ChangeType((UInt48)0xffff, typeof(ushort)));
      Assert.AreEqual((int)0x7fffffff, Convert.ChangeType((UInt48)0x7fffffff, typeof(int)));
      Assert.AreEqual((uint)0xffffffff, Convert.ChangeType((UInt48)0xffffffff, typeof(uint)));
      Assert.AreEqual((long)0x0000ffffffffffff, Convert.ChangeType((UInt48)0x0000ffffffffffff, typeof(long)));
      Assert.AreEqual((ulong)0x0000ffffffffffff, Convert.ChangeType((UInt48)0x0000ffffffffffff, typeof(ulong)));

      foreach (var t in new[] {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
      }) {
        Assert.Throws<OverflowException>(() => Convert.ChangeType(UInt48.MaxValue, t), t.FullName);
      }

      foreach (var t in new[] {
        typeof(long),
        typeof(ulong),
      }) {
        Assert.DoesNotThrow(() => Convert.ChangeType(UInt48.MaxValue, t), t.FullName);
      }
    }

    [Test]
    public void TestIConvertible_ToDateTime()
    {
      Assert.Throws<InvalidCastException>(() => Convert.ChangeType(UInt24.Zero, typeof(DateTime)));
      Assert.Throws<InvalidCastException>(() => ((IConvertible)UInt24.Zero).ToDateTime(provider: null));
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("0", UInt48.Zero.ToString());
      Assert.AreEqual("0000", UInt48.Zero.ToString("D4"));
      Assert.AreEqual("281474976710655", UInt48.MaxValue.ToString());
      Assert.AreEqual("FFFFFFFFFFFF", UInt48.MaxValue.ToString("X"));
    }

#if SYSTEM_ISPANFORMATTABLE
    [Test]
    public void TestTryFormat()
    {
      Span<char> destination = stackalloc char[12];
      int charsWritten = default;

      Assert.IsTrue(UInt48.Zero.TryFormat(destination, out charsWritten, ReadOnlySpan<char>.Empty, provider: null), "#1");
      Assert.AreEqual(1, charsWritten, $"#1 {nameof(charsWritten)}");
      Assert.AreEqual("0", new string(destination.Slice(0, charsWritten)), $"#1 formatted string");

      Assert.IsTrue(UInt48.Zero.TryFormat(destination, out charsWritten, "D4", provider: null), "#2");
      Assert.AreEqual(4, charsWritten, $"#2 {nameof(charsWritten)}");
      Assert.AreEqual("0000", new string(destination.Slice(0, charsWritten)), $"#2 formatted string");

      Assert.IsTrue(UInt48.MaxValue.TryFormat(destination, out charsWritten, "X", provider: null), "#3");
      Assert.AreEqual(12, charsWritten, $"#3 {nameof(charsWritten)}");
      Assert.AreEqual("FFFFFFFFFFFF", new string(destination.Slice(0, charsWritten)), $"#3 formatted string");
    }

    [Test]
    public void TestTryFormat_DestinationTooShort()
    {
      Assert.IsFalse(UInt48.Zero.TryFormat(Array.Empty<char>(), out var charsWritten, string.Empty, provider: null), "#1");
      Assert.IsFalse(UInt48.MaxValue.TryFormat(stackalloc char[UInt48.MaxValue.ToString().Length - 1], out charsWritten, string.Empty, provider: null), "#2");
    }
#endif
  }
}
