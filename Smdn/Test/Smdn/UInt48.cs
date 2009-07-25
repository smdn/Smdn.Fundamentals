using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class UInt48Tests {
    [Test]
    public void Test()
    {
      Assert.AreEqual((UInt48)(int)0x00ff00ff, (UInt48)(long)0x00ff00ff);

      var val = (UInt48)(long)0x001234567890;

      Assert.AreEqual((long)0x001234567890, val.ToInt64());

      Assert.AreEqual(new byte[] {0x00, 0x12, 0x34, 0x56, 0x78, 0x90}, val.ToBigEndianByteArray());
      Assert.AreEqual(new byte[] {0x90, 0x78, 0x56, 0x34, 0x12, 0x00}, val.ToLittleEndianByteArray());

      var zero = (UInt48)0x000000000000;
      var one  = (UInt48)0x000000000001;

      Assert.IsTrue(zero.Equals(zero));
      Assert.IsFalse(zero.Equals(one));
      Assert.IsFalse(zero.Equals(null));

      Assert.AreEqual(1, zero.CompareTo(null));

      Assert.Greater(one, zero);
    }
  }
}
