using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class BinaryConvertExtensionsTests {
    private void ExpectException<TException>(Action action) where TException : Exception
    {
      try {
        action();
        Assert.Fail("{0} not thrown", typeof(TException).Name);
      }
      catch (TException) {
      }
    }

    [Test]
    public void TestToUInt24()
    {
      Assert.AreEqual((UInt24)0xff0000,
                      BinaryConvertExtensions.ToUInt24(new byte[] {0xff, 0x00, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000cc,
                      BinaryConvertExtensions.ToUInt24(new byte[] {0xff, 0x00, 0x00, 0xcc}, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000ff,
                      BinaryConvertExtensions.ToUInt24(new byte[] {0xff, 0x00, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt24)0xcc0000,
                      BinaryConvertExtensions.ToUInt24(new byte[] {0xff, 0x00, 0x00, 0xcc}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvertExtensions.ToUInt24(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.ToUInt24(new byte[] {0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.ToUInt24(new byte[] {0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvertExtensions.ToUInt24(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt48()
    {
      Assert.AreEqual((UInt48)0xff0000000000,
                      BinaryConvertExtensions.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000cc,
                      BinaryConvertExtensions.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc}, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000ff,
                      BinaryConvertExtensions.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt48)0xcc0000000000,
                      BinaryConvertExtensions.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvertExtensions.ToUInt48(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvertExtensions.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestGetBytesUInt24()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd};

      BinaryConvertExtensions.GetBytes((UInt24)0x112233, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0xdd}, buffer);

      BinaryConvertExtensions.GetBytes((UInt24)0x112233, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x33, 0x22, 0x11, 0xdd}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33},
                                BinaryConvertExtensions.GetBytes((UInt24)0x112233, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x33, 0x22, 0x11},
                                BinaryConvertExtensions.GetBytes((UInt24)0x112233, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvertExtensions.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[2], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[3], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvertExtensions.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[3], 0));
    }

    [Test]
    public void TestGetBytesUInt48()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvertExtensions.GetBytes((UInt48)0x112233445566, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0xcc}, buffer);

      BinaryConvertExtensions.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66},
                                BinaryConvertExtensions.GetBytes((UInt48)0x112233445566, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConvertExtensions.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvertExtensions.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[5], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvertExtensions.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[6], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvertExtensions.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[6], 0));
    }
  }
}