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

      Assert.AreEqual(new byte[] {0x12, 0x34, 0x56}, val.ToBigEndianByteArray());
      Assert.AreEqual(new byte[] {0x56, 0x34, 0x12}, val.ToLittleEndianByteArray());

      var zero = (UInt24)0x000000;
      var one  = (UInt24)0x000001;

      Assert.IsTrue(zero.Equals(zero));
      Assert.IsFalse(zero.Equals(one));
      Assert.IsFalse(zero.Equals(null));

      Assert.AreEqual(1, zero.CompareTo(null));

      Assert.Greater(one, zero);
    }

    [Test]
    public void TestExplicitOperator()
    {
      int intval;

      intval = (UInt24)0x000000;
      Assert.AreEqual((int)0x000000, intval);

      intval = (UInt24)0xffffff;
      Assert.AreEqual((int)0xffffff, intval);

      uint uintval;

      uintval = (UInt24)0x000000;
      Assert.AreEqual((uint)0x000000, uintval);

      uintval = (UInt24)0xffffff;
      Assert.AreEqual((uint)0xffffff, uintval);
    }

    [Test]
    public void TestImplicitOperator()
    {
      Assert.AreEqual(0, (short)((UInt24)0x000000));
      Assert.AreEqual(short.MaxValue, (short)((UInt24)0x007fff));
      Assert.AreEqual(short.MinValue, (short)((UInt24)0x008000));
      Assert.AreEqual(-1, (short)((UInt24)0x00ffff));

      Assert.AreEqual((ushort)0x000000, (ushort)((UInt24)0x000000));
      Assert.AreEqual((ushort)0x007fff, (ushort)((UInt24)0x007fff));
      Assert.AreEqual((ushort)0x008000, (ushort)((UInt24)0x008000));
      Assert.AreEqual((ushort)0x00ffff, (ushort)((UInt24)0x00ffff));
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
        try {
          Convert.ChangeType(UInt24.MaxValue, t);
          Assert.Fail("OverflowException not thrown: type {0}", t);
        }
        catch (OverflowException) {
        }
      }

      foreach (var t in new[] {
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
      }) {
        try {
          Convert.ChangeType(UInt24.MaxValue, t);
        }
        catch (OverflowException) {
          Assert.Fail("OverflowException thrown: type {0}", t);
        }
      }
    }
  }
}
