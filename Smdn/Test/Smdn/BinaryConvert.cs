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
    public void TestByteSwap()
    {
      Assert.IsTrue(unchecked((long)0xefcdab8967452301) == BinaryConvert.ByteSwap(unchecked((long)0x0123456789abcdef)), "long, 0x0123456789abcdef");
      Assert.IsTrue(unchecked((ulong)0xefcdab8967452301) == BinaryConvert.ByteSwap(unchecked((ulong)0x0123456789abcdef)), "ulong, 0x0123456789abcdef");

      Assert.IsTrue(unchecked((long)0x1032547698badcfe) == BinaryConvert.ByteSwap(unchecked((long)0xfedcba9876543210)), "long, 0xfedcba9876543210");
      Assert.IsTrue(unchecked((ulong)0x1032547698badcfe) == BinaryConvert.ByteSwap(unchecked((ulong)0xfedcba9876543210)), "ulong, 0xfedcba9876543210");

      Assert.IsTrue(unchecked((int)0x78563412) == BinaryConvert.ByteSwap(unchecked((int)0x12345678)), "int, 0x12345678");
      Assert.IsTrue(unchecked((uint)0x78563412) == BinaryConvert.ByteSwap(unchecked((uint)0x12345678)), "uint, 0x12345678");

      Assert.IsTrue(unchecked((int)0x98badcfe) == BinaryConvert.ByteSwap(unchecked((int)0xfedcba98)), "int, 0xfedcba98");
      Assert.IsTrue(unchecked((uint)0x98badcfe) == BinaryConvert.ByteSwap(unchecked((uint)0xfedcba98)), "uint, 0xfedcba98");

      Assert.IsTrue(unchecked((short)0x3412) == BinaryConvert.ByteSwap(unchecked((short)0x1234)), "short, 0x1234");
      Assert.IsTrue(unchecked((ushort)0x3412) == BinaryConvert.ByteSwap(unchecked((ushort)0x1234)), "ushort, 0x1234");

      Assert.IsTrue(unchecked((short)0xdcfe) == BinaryConvert.ByteSwap(unchecked((short)0xfedc)), "short, 0xfedc");
      Assert.IsTrue(unchecked((ushort)0xdcfe) == BinaryConvert.ByteSwap(unchecked((ushort)0xfedc)), "ushort, 0xfedc");
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
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00},
                                BinaryConvert.GetBytes(short.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x80},
                                BinaryConvert.GetBytes(short.MinValue, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, new byte[1], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, new byte[2], 1));
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

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, new byte[1], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, new byte[2], 1));
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
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00},
                                BinaryConvert.GetBytes(int.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x80},
                                BinaryConvert.GetBytes(int.MinValue, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, new byte[3], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, new byte[4], 1));
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

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, new byte[3], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, new byte[4], 1));
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
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                BinaryConvert.GetBytes(long.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80},
                                BinaryConvert.GetBytes(long.MinValue, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, new byte[7], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, new byte[8], 1));
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

      ExpectException<ArgumentNullException>      (() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, new byte[7], 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, new byte[8], 1));
      ExpectException<NotSupportedException>      (() => BinaryConvert.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, new byte[8], 0));
    }
  }
}

