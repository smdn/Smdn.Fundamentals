using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UInt48Tests {
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
  }
}
