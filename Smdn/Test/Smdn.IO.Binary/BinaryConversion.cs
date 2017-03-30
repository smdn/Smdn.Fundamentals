using System;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture()]
  public class BinaryConversionTests {
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
      Assert.IsTrue(unchecked((long)0xefcdab8967452301) == BinaryConversion.ByteSwap(unchecked((long)0x0123456789abcdef)), "long, 0x0123456789abcdef");
      Assert.IsTrue(unchecked((ulong)0xefcdab8967452301) == BinaryConversion.ByteSwap(unchecked((ulong)0x0123456789abcdef)), "ulong, 0x0123456789abcdef");

      Assert.IsTrue(unchecked((long)0x1032547698badcfe) == BinaryConversion.ByteSwap(unchecked((long)0xfedcba9876543210)), "long, 0xfedcba9876543210");
      Assert.IsTrue(unchecked((ulong)0x1032547698badcfe) == BinaryConversion.ByteSwap(unchecked((ulong)0xfedcba9876543210)), "ulong, 0xfedcba9876543210");

      Assert.IsTrue(unchecked((int)0x78563412) == BinaryConversion.ByteSwap(unchecked((int)0x12345678)), "int, 0x12345678");
      Assert.IsTrue(unchecked((uint)0x78563412) == BinaryConversion.ByteSwap(unchecked((uint)0x12345678)), "uint, 0x12345678");

      Assert.IsTrue(unchecked((int)0x98badcfe) == BinaryConversion.ByteSwap(unchecked((int)0xfedcba98)), "int, 0xfedcba98");
      Assert.IsTrue(unchecked((uint)0x98badcfe) == BinaryConversion.ByteSwap(unchecked((uint)0xfedcba98)), "uint, 0xfedcba98");

      Assert.IsTrue(unchecked((short)0x3412) == BinaryConversion.ByteSwap(unchecked((short)0x1234)), "short, 0x1234");
      Assert.IsTrue(unchecked((ushort)0x3412) == BinaryConversion.ByteSwap(unchecked((ushort)0x1234)), "ushort, 0x1234");

      Assert.IsTrue(unchecked((short)0xdcfe) == BinaryConversion.ByteSwap(unchecked((short)0xfedc)), "short, 0xfedc");
      Assert.IsTrue(unchecked((ushort)0xdcfe) == BinaryConversion.ByteSwap(unchecked((ushort)0xfedc)), "ushort, 0xfedc");
    }

    [Test]
    public void TestToInt16()
    {
      Assert.AreEqual(unchecked((short)0x8000),
                      BinaryConversion.ToInt16(new byte[] {0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((short)0x8000),
                      BinaryConversion.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((short)0x0080),
                      BinaryConversion.ToInt16(new byte[] {0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((short)0x0080),
                      BinaryConversion.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((short)0x8000),
                      BinaryConversion.ToInt16BE(new byte[] {0x80, 0x00}, 0));
      Assert.AreEqual(unchecked((short)0x0080),
                      BinaryConversion.ToInt16LE(new byte[] {0x80, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToInt16(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt16(new byte[] {0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt16(new byte[] {0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt16(new byte[] {0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToInt16(new byte[] {0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt16()
    {
      Assert.AreEqual(unchecked((ushort)0x8000),
                      BinaryConversion.ToUInt16(new byte[] {0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ushort)0x8000),
                      BinaryConversion.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ushort)0x0080),
                      BinaryConversion.ToUInt16(new byte[] {0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((ushort)0x0080),
                      BinaryConversion.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((ushort)0x8000),
                      BinaryConversion.ToUInt16BE(new byte[] {0x80, 0x00}, 0));
      Assert.AreEqual(unchecked((ushort)0x0080),
                      BinaryConversion.ToUInt16LE(new byte[] {0x80, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToUInt16(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt16(new byte[] {0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt16(new byte[] {0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt16(new byte[] {0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToUInt16(new byte[] {0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToInt32()
    {
      Assert.AreEqual(unchecked((int)0xff008000),
                      BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((int)0x00800000),
                      BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((int)0x008000ff),
                      BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((int)0x00008000),
                      BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((int)0xff008000),
                      BinaryConversion.ToInt32BE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0));
      Assert.AreEqual(unchecked((int)0x008000ff),
                      BinaryConversion.ToInt32LE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToInt32(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.AreEqual(unchecked((uint)0xff008000),
                      BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((uint)0x00800000),
                      BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((uint)0x008000ff),
                      BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((uint)0x00008000),
                      BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((uint)0xff008000),
                      BinaryConversion.ToUInt32BE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0));
      Assert.AreEqual(unchecked((uint)0x008000ff),
                      BinaryConversion.ToUInt32LE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToUInt32(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToInt64()
    {
      Assert.AreEqual(unchecked((long)0xff00cc008000dd00),
                      BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((long)0x00cc008000dd0000),
                      BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((long)0x00dd008000cc00ff),
                      BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((long)0x0000dd008000cc00),
                      BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((long)0xff00cc008000dd00),
                      BinaryConversion.ToInt64BE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0));
      Assert.AreEqual(unchecked((long)0x00dd008000cc00ff),
                      BinaryConversion.ToInt64LE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToInt64(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.AreEqual(unchecked((ulong)0xff00cc008000dd00),
                      BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ulong)0x00cc008000dd0000),
                      BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.BigEndian));
      Assert.AreEqual(unchecked((ulong)0x00dd008000cc00ff),
                      BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, Endianness.LittleEndian));
      Assert.AreEqual(unchecked((ulong)0x0000dd008000cc00),
                      BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, Endianness.LittleEndian));

      Assert.AreEqual(unchecked((ulong)0xff00cc008000dd00),
                      BinaryConversion.ToUInt64BE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0));
      Assert.AreEqual(unchecked((ulong)0x00dd008000cc00ff),
                      BinaryConversion.ToUInt64LE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.ToUInt64(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>          (() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>      (() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, Endianness.Unknown));
    }

    [Test]
    public void TestGetBytesInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0xff, 0x00, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x00, 0xff, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0xff, 0x00},
                                BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0xff},
                                BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00},
                                BinaryConversion.GetBytes(short.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x80},
                                BinaryConversion.GetBytes(short.MinValue, Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00},
                                BinaryConversion.GetBytesBE(short.MinValue));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x80},
                                BinaryConversion.GetBytesLE(short.MinValue));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, new byte[1], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, new byte[1], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.BigEndian, new byte[2], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((short)0xff00), Endianness.Unknown, new byte[2], 0));
    }

    [Test]
    public void TestGetBytesUInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0xff, 0x00, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x00, 0xff, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0xff, 0x00},
                                BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0xff},
                                BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0xff, 0x00},
                                BinaryConversion.GetBytesBE(unchecked((ushort)0xff00)));
      CollectionAssert.AreEqual(new byte[] {0x00, 0xff},
                                BinaryConversion.GetBytesLE(unchecked((ushort)0xff00)));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, new byte[1], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, new byte[1], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.BigEndian, new byte[2], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), Endianness.Unknown, new byte[2], 0));
    }

    [Test]
    public void TestGetBytesInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44},
                                BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00},
                                BinaryConversion.GetBytes(int.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x80},
                                BinaryConversion.GetBytes(int.MinValue, Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00},
                                BinaryConversion.GetBytesBE(int.MinValue));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x80},
                                BinaryConversion.GetBytesLE(int.MinValue));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, new byte[3], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, new byte[3], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.BigEndian, new byte[4], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), Endianness.Unknown, new byte[4], 0));
    }

    [Test]
    public void TestGetBytesUInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44},
                                BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44},
                                BinaryConversion.GetBytesBE(unchecked((uint)0x11223344)));
      CollectionAssert.AreEqual(new byte[] {0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytesLE(unchecked((uint)0x11223344)));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, new byte[3], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, new byte[3], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.BigEndian, new byte[4], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), Endianness.Unknown, new byte[4], 0));
    }

    [Test]
    public void TestGetBytesInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88},
                                BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                BinaryConversion.GetBytes(long.MinValue, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80},
                                BinaryConversion.GetBytes(long.MinValue, Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                BinaryConversion.GetBytesBE(long.MinValue));
      CollectionAssert.AreEqual(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80},
                                BinaryConversion.GetBytesLE(long.MinValue));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, new byte[7], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, new byte[7], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.BigEndian, new byte[8], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), Endianness.Unknown, new byte[8], 0));
    }

    [Test]
    public void TestGetBytesUInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}, buffer);

      BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}, buffer);

      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88},
                                BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.LittleEndian));
      CollectionAssert.AreEqual(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88},
                                BinaryConversion.GetBytesBE(unchecked((ulong)0x1122334455667788)));
      CollectionAssert.AreEqual(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11},
                                BinaryConversion.GetBytesLE(unchecked((ulong)0x1122334455667788)));

      ExpectException<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, new byte[7], -1));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, new byte[7], 0));
      ExpectException<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.BigEndian, new byte[8], 1));
      ExpectException<NotSupportedException>      (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), Endianness.Unknown, new byte[8], 0));
    }

    [Test]
    public void TestToUInt24()
    {
      Assert.AreEqual((UInt24)0xff0000,
                      BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000cc,
                      BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt24)0x0000ff,
                      BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt24)0xcc0000,
                      BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>(() => BinaryConversion.ToUInt24(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00 }, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00 }, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00, 0x00 }, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00, 0x00 }, 0, Endianness.Unknown));
    }

    [Test]
    public void TestToUInt48()
    {
      Assert.AreEqual((UInt48)0xff0000000000,
                      BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000cc,
                      BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, Endianness.BigEndian));
      Assert.AreEqual((UInt48)0x0000000000ff,
                      BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, Endianness.LittleEndian));
      Assert.AreEqual((UInt48)0xcc0000000000,
                      BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>(() => BinaryConversion.ToUInt48(null, 0, Endianness.BigEndian));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, -1, Endianness.BigEndian));
      ExpectException<ArgumentException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, Endianness.BigEndian));
      ExpectException<ArgumentException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 1, Endianness.BigEndian));
      ExpectException<NotSupportedException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, Endianness.Unknown));
    }

    [Test]
    public void TestGetBytesUInt24()
    {
      var buffer = new byte[] { 0xdd, 0xcc, 0xdd, 0xcc, 0xdd };

      BinaryConversion.GetBytes((UInt24)0x112233, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] { 0xdd, 0x11, 0x22, 0x33, 0xdd }, buffer);

      BinaryConversion.GetBytes((UInt24)0x112233, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] { 0xdd, 0x33, 0x22, 0x11, 0xdd }, buffer);

      CollectionAssert.AreEqual(new byte[] { 0x11, 0x22, 0x33 },
                                BinaryConversion.GetBytes((UInt24)0x112233, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] { 0x33, 0x22, 0x11 },
                                BinaryConversion.GetBytes((UInt24)0x112233, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), Endianness.BigEndian, new byte[2], -1));
      ExpectException<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), Endianness.BigEndian, new byte[2], 0));
      ExpectException<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), Endianness.BigEndian, new byte[3], 1));
      ExpectException<NotSupportedException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), Endianness.Unknown, new byte[3], 0));
    }

    [Test]
    public void TestGetBytesUInt48()
    {
      var buffer = new byte[] { 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc };

      BinaryConversion.GetBytes((UInt48)0x112233445566, Endianness.BigEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] { 0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0xcc }, buffer);

      BinaryConversion.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian, buffer, 1);

      CollectionAssert.AreEqual(new byte[] { 0xdd, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc }, buffer);

      CollectionAssert.AreEqual(new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66 },
                                BinaryConversion.GetBytes((UInt48)0x112233445566, Endianness.BigEndian));
      CollectionAssert.AreEqual(new byte[] { 0x66, 0x55, 0x44, 0x33, 0x22, 0x11 },
                                BinaryConversion.GetBytes((UInt48)0x112233445566, Endianness.LittleEndian));

      ExpectException<ArgumentNullException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), Endianness.BigEndian, null, 0));
      ExpectException<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), Endianness.BigEndian, new byte[5], -1));
      ExpectException<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), Endianness.BigEndian, new byte[5], 0));
      ExpectException<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), Endianness.BigEndian, new byte[6], 1));
      ExpectException<NotSupportedException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), Endianness.Unknown, new byte[6], 0));
    }
  }
}

