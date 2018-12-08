using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UInt24Tests {
    [Test]
    public void TestSizeOfStructure()
    {
      Assert.AreEqual(3, System.Runtime.InteropServices.Marshal.SizeOf(typeof(UInt24)));
    }

    [Test]
    public void Test()
    {
      Assert.AreEqual((UInt24)(int)0xff00ff, (UInt24)0xff00ff);

      var val = (UInt24)(int)0x123456;

      Assert.AreEqual((int)0x123456, val.ToInt32());

      var zero = (UInt24)0x000000;
      var one  = (UInt24)0x000001;

      Assert.IsTrue(zero.Equals(zero));
      Assert.IsFalse(zero.Equals(one));
      Assert.IsFalse(zero.Equals(null));

      Assert.AreEqual(1, zero.CompareTo(null));

      Assert.That(0 < one.CompareTo(zero));
      Assert.That(0 < one.CompareTo(0));
      Assert.That(0 < one.CompareTo(0u));
    }

    [Test]
    public void TestEquals()
    {
      Assert.IsTrue(UInt24.Zero.Equals(UInt24.Zero));
      Assert.IsTrue(UInt24.Zero.Equals(0));
      Assert.IsTrue(UInt24.Zero.Equals(0u));
      Assert.IsFalse(UInt24.Zero.Equals(UInt24.MaxValue));
      Assert.IsFalse(UInt24.Zero.Equals(int.MaxValue));
      Assert.IsFalse(UInt24.Zero.Equals(uint.MaxValue));

      object val;

      val = UInt24.Zero;
      Assert.IsTrue(UInt24.Zero.Equals(val));

      val = 0;
      Assert.IsTrue(UInt24.Zero.Equals(val));

      val = 0u;
      Assert.IsTrue(UInt24.Zero.Equals(val));
    }

    [Test]
    public void TestOpEquality()
    {
      Assert.IsTrue(UInt24.Zero == (UInt24)0);
      Assert.IsFalse(UInt24.Zero == (UInt24)0x000010);
      Assert.IsFalse(UInt24.Zero == (UInt24)0x001000);
      Assert.IsFalse(UInt24.Zero == (UInt24)0x100000);
    }

    [Test]
    public void TestOpIneqality()
    {
      Assert.IsFalse(UInt24.Zero != (UInt24)0);
      Assert.IsTrue(UInt24.Zero != (UInt24)0x000010);
      Assert.IsTrue(UInt24.Zero != (UInt24)0x001000);
      Assert.IsTrue(UInt24.Zero != (UInt24)0x100000);
    }

    [Test]
    public void TestOpExplicitFromInt16()
    {
      foreach (var test in new[] {
        new {Value = (short)0x000000, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (short)0x007fff, ExpectedResult = 0x00007fff, ExpectedHex = "7fff"},
        new {Value = Int16.MaxValue,  ExpectedResult = 0x00007fff, ExpectedHex = "7fff"},
      }) {

        try {
          UInt24 val = (UInt24)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt32(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (short)-1},
        new {Value = Int16.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = (UInt24)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitFromInt32()
    {
      foreach (var test in new[] {
        new {Value = (int)0x00000000, ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (int)0x00ffffff, ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
      }) {

        try {
          UInt24 val = (UInt24)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt32(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (int)-1},
        new {Value = (int)0x01000000},
        new {Value = Int32.MaxValue},
        new {Value = Int32.MinValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = (UInt24)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitFromUInt32()
    {
      foreach (var test in new[] {
        new {Value = (uint)0x00000000,  ExpectedResult = 0x00000000, ExpectedHex = "0"},
        new {Value = (uint)0x00ffffff,  ExpectedResult = 0x00ffffff, ExpectedHex = "ffffff"},
        new {Value = UInt32.MinValue,   ExpectedResult = 0x00000000, ExpectedHex = "0"},
      }) {

        try {
          UInt24 val = (UInt24)test.Value;

          Assert.IsTrue(test.ExpectedResult == val.ToInt32(), "value = {0}", test.ExpectedHex);
          Assert.AreEqual(test.ExpectedHex, val.ToString("x"));
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (uint)0x01000000},
        new {Value = (uint)0xffffffff},
        new {Value = UInt32.MaxValue},
      }) {
        Assert.Throws<OverflowException>(() => { UInt24 val = (UInt24)test.Value; });
      }
    }

    [Test]
    public void TestOpExplicitToInt16()
    {
      foreach (var test in new[] {
        new {Value = UInt24.MinValue,   ExpectedResult = (short)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x000000,  ExpectedResult = (short)0x0000, ExpectedHex = "0"},
        new {Value = (UInt24)0x007fff,  ExpectedResult = (short)0x7fff, ExpectedHex = "7fff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        try {
          Assert.IsTrue(test.ExpectedResult == (short)test.Value);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (UInt24)0x008000,  ExpectedHex = "8000"},
        new {Value = (UInt24)0xffffff,  ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedHex = "ffffff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        Assert.Throws<OverflowException>(() => { var s = (short)test.Value; });
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
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        try {
          Assert.IsTrue(test.ExpectedResult == (ushort)test.Value);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: value = {0}", test.ExpectedHex);
        }
      }

      foreach (var test in new[] {
        new {Value = (UInt24)0x010000,  ExpectedHex = "10000"},
        new {Value = (UInt24)0xffffff,  ExpectedHex = "ffffff"},
        new {Value = UInt24.MaxValue,   ExpectedHex = "ffffff"},
      }) {
        Assert.AreEqual(test.ExpectedHex, test.Value.ToString("x"));

        Assert.Throws<OverflowException>(() => { var us = (ushort)test.Value; });
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

        Assert.IsTrue(test.ExpectedResult == val.ToInt32(), "value = {0}", val);
        Assert.AreEqual(test.ExpectedHex, val.ToString("x"), "value = {0}", val);
      }
    }

    [Test]
    public void TestOpExplicitToInt32()
    {
      int max = (int)UInt24.MaxValue;
      int min = (int)UInt24.MinValue;

      Assert.IsTrue((int)0x00ffffff == max);
      Assert.IsTrue((int)0x00000000 == min);
    }

    [Test]
    public void TestOpExplicitToUInt32()
    {
      uint max = (uint)UInt24.MaxValue;
      uint min = (uint)UInt24.MinValue;

      Assert.IsTrue((uint)0x00ffffff == max);
      Assert.IsTrue((uint)0x00000000 == min);
    }

    [Test]
    public void TestToInt32()
    {
      Assert.IsTrue((int)0x00ffffff == UInt24.MaxValue.ToInt32());
      Assert.IsTrue((int)0x00000000 == UInt24.MinValue.ToInt32());

      UInt24 val = (UInt24)0x123456;

      Assert.IsTrue((int)0x00123456 == val.ToInt32());
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.IsTrue((uint)0x00ffffff == UInt24.MaxValue.ToUInt32());
      Assert.IsTrue((uint)0x00000000 == UInt24.MinValue.ToUInt32());

      UInt24 val = (UInt24)0x123456;

      Assert.IsTrue((uint)0x00123456 == val.ToUInt32());
    }

    [Test]
    public void TestIConvertible()
    {
      Assert.AreEqual(true, Convert.ChangeType((UInt24)1, typeof(bool)));
      Assert.AreEqual(false, Convert.ChangeType((UInt24)0, typeof(bool)));
      Assert.AreEqual((byte)0xff, Convert.ChangeType((UInt24)0xff, typeof(byte)));
      Assert.AreEqual((sbyte)0x7f, Convert.ChangeType((UInt24)0x7f, typeof(sbyte)));
      Assert.AreEqual((short)0x7fff, Convert.ChangeType((UInt24)0x7fff, typeof(short)));
      Assert.AreEqual((ushort)0xffff, Convert.ChangeType((UInt24)0xffff, typeof(ushort)));
      Assert.AreEqual((int)0x00ffffff, Convert.ChangeType((UInt24)0xffffff, typeof(int)));
      Assert.AreEqual((uint)0x00ffffff, Convert.ChangeType((UInt24)0xffffff, typeof(uint)));
      Assert.AreEqual((long)0x0000000000ffffff, Convert.ChangeType((UInt24)0xffffff, typeof(long)));
      Assert.AreEqual((ulong)0x0000000000ffffff, Convert.ChangeType((UInt24)0xffffff, typeof(ulong)));

      foreach (var t in new[] {
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
      }) {
        Assert.Throws<OverflowException>(() => Convert.ChangeType(UInt24.MaxValue, t), t.FullName);
      }

      foreach (var t in new[] {
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
      }) {
        Assert.DoesNotThrow(() => Convert.ChangeType(UInt24.MaxValue, t), t.FullName);
      }
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("0", UInt24.Zero.ToString());
      Assert.AreEqual("0000", UInt24.Zero.ToString("D4"));
      Assert.AreEqual("16777215", UInt24.MaxValue.ToString());
      Assert.AreEqual("FFFFFF", UInt24.MaxValue.ToString("X"));
    }
  }
}
