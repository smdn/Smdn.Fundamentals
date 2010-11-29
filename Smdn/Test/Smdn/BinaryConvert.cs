using System;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class BinaryConvertTests {
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
    public void TestToInt16()
    {
      Assert.AreEqual(unchecked((short)0x8000),
                      BinaryConvert.ToInt16(new byte[] {0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((short)0x8000),
                      BinaryConvert.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((short)0x0080),
                      BinaryConvert.ToInt16(new byte[] {0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((short)0x0080),
                      BinaryConvert.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToInt16(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt16(new byte[] {0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt16(new byte[] {0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToInt16(new byte[] {0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt16()
    {
      Assert.AreEqual(unchecked((ushort)0x8000),
                      BinaryConvert.ToUInt16(new byte[] {0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ushort)0x8000),
                      BinaryConvert.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ushort)0x0080),
                      BinaryConvert.ToUInt16(new byte[] {0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((ushort)0x0080),
                      BinaryConvert.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToUInt16(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt16(new byte[] {0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt16(new byte[] {0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToUInt16(new byte[] {0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToInt32()
    {
      Assert.AreEqual(unchecked((int)0xff008000),
                      BinaryConvert.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((int)0x00800000),
                      BinaryConvert.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((int)0x008000ff),
                      BinaryConvert.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((int)0x00008000),
                      BinaryConvert.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToInt32(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt32(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.AreEqual(unchecked((uint)0xff008000),
                      BinaryConvert.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((uint)0x00800000),
                      BinaryConvert.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((uint)0x008000ff),
                      BinaryConvert.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((uint)0x00008000),
                      BinaryConvert.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToUInt32(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt32(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToUInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToInt64()
    {
      Assert.AreEqual(unchecked((long)0xff00cc008000dd00),
                      BinaryConvert.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((long)0x00cc008000dd0000),
                      BinaryConvert.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((long)0x00dd008000cc00ff),
                      BinaryConvert.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((long)0x0000dd008000cc00),
                      BinaryConvert.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToInt64(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.AreEqual(unchecked((ulong)0xff00cc008000dd00),
                      BinaryConvert.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ulong)0x00cc008000dd0000),
                      BinaryConvert.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ulong)0x00dd008000cc00ff),
                      BinaryConvert.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((ulong)0x0000dd008000cc00),
                      BinaryConvert.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToUInt64(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt24()
    {
      Assert.AreEqual((UInt24)0xff0000,
                      BinaryConvert.ToUInt24(new byte[] {0xff, 0x00, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000cc,
                      BinaryConvert.ToUInt24(new byte[] {0xff, 0x00, 0x00, 0xcc}, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000ff,
                      BinaryConvert.ToUInt24(new byte[] {0xff, 0x00, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt24)0xcc0000,
                      BinaryConvert.ToUInt24(new byte[] {0xff, 0x00, 0x00, 0xcc}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToUInt24(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt24(new byte[] {0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt24(new byte[] {0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToUInt24(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt48()
    {
      Assert.AreEqual((UInt48)0xff0000000000,
                      BinaryConvert.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000cc,
                      BinaryConvert.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc}, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000ff,
                      BinaryConvert.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt48)0xcc0000000000,
                      BinaryConvert.ToUInt48(new byte[] {0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc}, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.ToUInt48(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConvert.ToUInt48(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestGetBytesInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0xff, 0x00, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x00, 0xff, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0xff, 0x00},
                                BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0xff},
                                BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.Unknown, new byte[1], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.Unknown, new byte[2], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.Unknown, new byte[2], 0));
    }

    [Test]
    public void TestGetBytesUInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0xff, 0x00, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x00, 0xff, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0xff, 0x00},
                                BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0xff},
                                BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.Unknown, new byte[1], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.Unknown, new byte[2], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.Unknown, new byte[2], 0));
    }

    [Test]
    public void TestGetBytesInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44},
                                BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x44, 0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.Unknown, new byte[3], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.Unknown, new byte[4], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.Unknown, new byte[4], 0));
    }

    [Test]
    public void TestGetBytesUInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44},
                                BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x44, 0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.Unknown, new byte[3], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.Unknown, new byte[4], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.Unknown, new byte[4], 0));
    }

    [Test]
    public void TestGetBytesInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88},
                                BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.Unknown, new byte[7], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.Unknown, new byte[8], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.Unknown, new byte[8], 0));
    }

    [Test]
    public void TestGetBytesUInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}, buffer);

      BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88},
                                BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, new byte[7], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, new byte[8], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, new byte[8], 0));
    }

    [Test]
    public void TestGetBytesUInt24()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd};

      BinaryConvert.GetBytes((UInt24)0x112233, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0xdd}, buffer);

      BinaryConvert.GetBytes((UInt24)0x112233, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x33, 0x22, 0x11, 0xdd}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33},
                                BinaryConvert.GetBytes((UInt24)0x112233, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes((UInt24)0x112233, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[2], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[3], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[3], 0));
    }

    [Test]
    public void TestGetBytesUInt48()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConvert.GetBytes((UInt48)0x112233445566, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0xcc}, buffer);

      BinaryConvert.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66},
                                BinaryConvert.GetBytes((UInt48)0x112233445566, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConvert.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[5], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[6], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[6], 0));
    }
  }
}

