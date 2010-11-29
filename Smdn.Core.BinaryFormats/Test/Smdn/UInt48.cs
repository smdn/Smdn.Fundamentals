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

      Assert.Less(zero, one);
      Assert.Less(0L, one);
      Assert.Less(0UL, one);
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
        try {
          UInt48 val = (UInt48)test.Value;

          Assert.Fail("OverflowException not thrown: value = {0}", test.Value);
          Assert.AreNotEqual(0L, val.ToInt64());
        }
        catch (OverflowException) {
        }
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
        try {
          UInt48 val = (UInt48)test.Value;

          Assert.Fail("OverflowException not thrown: value = {0}", test.Value);
          Assert.AreNotEqual(0L, val.ToInt64());
        }
        catch (OverflowException) {
        }
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
        try {
          UInt48 val = (UInt48)test.Value;

          Assert.Fail("OverflowException not thrown: value = {0}", test.Value);
          Assert.AreNotEqual(0UL, val.ToUInt64());
        }
        catch (OverflowException) {
        }
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

        try {
          Assert.IsFalse((int)0 == (int)test.Value);
          Assert.Fail("OverflowException not thrown: value = {0}", test.ExpectedHex);
        }
        catch (OverflowException) {
        }
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

        try {
          Assert.IsFalse((uint)0 == (uint)test.Value);
          Assert.Fail("OverflowException not thrown: value = {0}", test.ExpectedHex);
        }
        catch (OverflowException) {
        }
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
        try {
          Convert.ChangeType(UInt48.MaxValue, t);
          Assert.Fail("OverflowException not thrown: type {0}", t);
        }
        catch (OverflowException) {
        }
      }

      foreach (var t in new[] {
        typeof(long),
        typeof(ulong),
      }) {
        try {
          Convert.ChangeType(UInt48.MaxValue, t);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: type {0}", t);
        }
      }
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("0", UInt48.Zero.ToString());
      Assert.AreEqual("0000", UInt48.Zero.ToString("D4"));
      Assert.AreEqual("281474976710655", UInt48.MaxValue.ToString());
      Assert.AreEqual("FFFFFFFFFFFF", UInt48.MaxValue.ToString("X"));
    }
  }
}
