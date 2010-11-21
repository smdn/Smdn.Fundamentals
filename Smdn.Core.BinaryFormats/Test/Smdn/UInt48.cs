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

      Assert.AreEqual(new byte[] {0x12, 0x34, 0x56, 0x78, 0x90, 0x12}, val.ToBigEndianByteArray());
      Assert.AreEqual(new byte[] {0x12, 0x90, 0x78, 0x56, 0x34, 0x12}, val.ToLittleEndianByteArray());

      var zero = (UInt48)0x000000000000;
      var one  = (UInt48)0x000000000001;

      Assert.IsTrue(zero.Equals(zero));
      Assert.IsFalse(zero.Equals(one));
      Assert.IsFalse(zero.Equals(null));

      Assert.AreEqual(1, zero.CompareTo(null));

      Assert.Greater(one, zero);
    }

    [Test]
    public void TestExplicitOperator()
    {
      long longval;

      longval = (UInt48)0x000000000000;
      Assert.AreEqual((long)0x000000000000, longval);

      longval = (UInt48)0xffffffffffff;
      Assert.AreEqual((long)0xffffffffffff, longval);

      ulong ulongval;

      ulongval = (UInt48)0x000000000000;
      Assert.AreEqual((ulong)0x000000000000, ulongval);

      ulongval = (UInt48)0xffffffffffff;
      Assert.AreEqual((ulong)0xffffffffffff, ulongval);
    }

    [Test]
    public void TestImplicitOperator()
    {
      Assert.AreEqual(0, (int)((UInt48)0x000000000000));
      Assert.AreEqual(int.MaxValue, (int)((UInt48)0x00007fffffff));
      Assert.AreEqual(int.MinValue, (int)((UInt48)0x000080000000));
      Assert.AreEqual(-1, (int)((UInt48)0x0000ffffffff));

      Assert.AreEqual((uint)0x000000000000, (uint)((UInt48)0x000000000000));
      Assert.AreEqual((uint)0x00007fffffff, (uint)((UInt48)0x00007fffffff));
      Assert.AreEqual((uint)0x000080000000, (uint)((UInt48)0x000080000000));
      Assert.AreEqual((uint)0x0000ffffffff, (uint)((UInt48)0x0000ffffffff));
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
  }
}
