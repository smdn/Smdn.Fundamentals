// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture()]
  public class BinaryConversionTests {
    [Test]
    public void TestByteSwap()
    {
      Assert.That(unchecked((long)0xefcdab8967452301) == BinaryConversion.ByteSwap(unchecked((long)0x0123456789abcdef)), Is.True, "long, 0x0123456789abcdef");
      Assert.That(unchecked((ulong)0xefcdab8967452301) == BinaryConversion.ByteSwap(unchecked((ulong)0x0123456789abcdef)), Is.True, "ulong, 0x0123456789abcdef");

      Assert.That(unchecked((long)0x1032547698badcfe) == BinaryConversion.ByteSwap(unchecked((long)0xfedcba9876543210)), Is.True, "long, 0xfedcba9876543210");
      Assert.That(unchecked((ulong)0x1032547698badcfe) == BinaryConversion.ByteSwap(unchecked((ulong)0xfedcba9876543210)), Is.True, "ulong, 0xfedcba9876543210");

      Assert.That(unchecked((int)0x78563412) == BinaryConversion.ByteSwap(unchecked((int)0x12345678)), Is.True, "int, 0x12345678");
      Assert.That(unchecked((uint)0x78563412) == BinaryConversion.ByteSwap(unchecked((uint)0x12345678)), Is.True, "uint, 0x12345678");

      Assert.That(unchecked((int)0x98badcfe) == BinaryConversion.ByteSwap(unchecked((int)0xfedcba98)), Is.True, "int, 0xfedcba98");
      Assert.That(unchecked((uint)0x98badcfe) == BinaryConversion.ByteSwap(unchecked((uint)0xfedcba98)), Is.True, "uint, 0xfedcba98");

      Assert.That(unchecked((short)0x3412) == BinaryConversion.ByteSwap(unchecked((short)0x1234)), Is.True, "short, 0x1234");
      Assert.That(unchecked((ushort)0x3412) == BinaryConversion.ByteSwap(unchecked((ushort)0x1234)), Is.True, "ushort, 0x1234");

      Assert.That(unchecked((short)0xdcfe) == BinaryConversion.ByteSwap(unchecked((short)0xfedc)), Is.True, "short, 0xfedc");
      Assert.That(unchecked((ushort)0xdcfe) == BinaryConversion.ByteSwap(unchecked((ushort)0xfedc)), Is.True, "ushort, 0xfedc");
    }

    [Test]
    public void TestToInt16()
    {
      Assert.That(BinaryConversion.ToInt16(new byte[] {0x80, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((short)0x8000)));
      Assert.That(BinaryConversion.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((short)0x8000)));
      Assert.That(BinaryConversion.ToInt16(new byte[] {0x80, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((short)0x0080)));
      Assert.That(BinaryConversion.ToInt16(new byte[] {0x00, 0x80, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((short)0x0080)));

      Assert.That(BinaryConversion.ToInt16BE(new byte[] {0x80, 0x00}, 0), Is.EqualTo(unchecked((short)0x8000)));
      Assert.That(BinaryConversion.ToInt16LE(new byte[] {0x80, 0x00}, 0), Is.EqualTo(unchecked((short)0x0080)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToInt16(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt16(new byte[] {0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt16(new byte[] {0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt16(new byte[] {0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToUInt16()
    {
      Assert.That(BinaryConversion.ToUInt16(new byte[] {0x80, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((ushort)0x8000)));
      Assert.That(BinaryConversion.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((ushort)0x8000)));
      Assert.That(BinaryConversion.ToUInt16(new byte[] {0x80, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((ushort)0x0080)));
      Assert.That(BinaryConversion.ToUInt16(new byte[] {0x00, 0x80, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((ushort)0x0080)));

      Assert.That(BinaryConversion.ToUInt16BE(new byte[] {0x80, 0x00}, 0), Is.EqualTo(unchecked((ushort)0x8000)));
      Assert.That(BinaryConversion.ToUInt16LE(new byte[] {0x80, 0x00}, 0), Is.EqualTo(unchecked((ushort)0x0080)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToUInt16(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt16(new byte[] {0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt16(new byte[] {0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt16(new byte[] {0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToInt32()
    {
      Assert.That(BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((int)0xff008000)));
      Assert.That(BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((int)0x00800000)));
      Assert.That(BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((int)0x008000ff)));
      Assert.That(BinaryConversion.ToInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((int)0x00008000)));

      Assert.That(BinaryConversion.ToInt32BE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0), Is.EqualTo(unchecked((int)0xff008000)));
      Assert.That(BinaryConversion.ToInt32LE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0), Is.EqualTo(unchecked((int)0x008000ff)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToInt32(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.That(BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((uint)0xff008000)));
      Assert.That(BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((uint)0x00800000)));
      Assert.That(BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((uint)0x008000ff)));
      Assert.That(BinaryConversion.ToUInt32(new byte[] {0xff, 0x00, 0x80, 0x00, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((uint)0x00008000)));

      Assert.That(BinaryConversion.ToUInt32BE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0), Is.EqualTo(unchecked((uint)0xff008000)));
      Assert.That(BinaryConversion.ToUInt32LE(new byte[] {0xff, 0x00, 0x80, 0x00}, 0), Is.EqualTo(unchecked((uint)0x008000ff)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToUInt32(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt32(new byte[] {0x00, 0x00, 0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToInt64()
    {
      Assert.That(BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((long)0xff00cc008000dd00)));
      Assert.That(BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((long)0x00cc008000dd0000)));
      Assert.That(BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((long)0x00dd008000cc00ff)));
      Assert.That(BinaryConversion.ToInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((long)0x0000dd008000cc00)));

      Assert.That(BinaryConversion.ToInt64BE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0), Is.EqualTo(unchecked((long)0xff00cc008000dd00)));
      Assert.That(BinaryConversion.ToInt64LE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0), Is.EqualTo(unchecked((long)0x00dd008000cc00ff)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToInt64(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.That(BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, asLittleEndian: false), Is.EqualTo(unchecked((ulong)0xff00cc008000dd00)));
      Assert.That(BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, asLittleEndian: false), Is.EqualTo(unchecked((ulong)0x00cc008000dd0000)));
      Assert.That(BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0, asLittleEndian: true), Is.EqualTo(unchecked((ulong)0x00dd008000cc00ff)));
      Assert.That(BinaryConversion.ToUInt64(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00, 0x00}, 1, asLittleEndian: true), Is.EqualTo(unchecked((ulong)0x0000dd008000cc00)));

      Assert.That(BinaryConversion.ToUInt64BE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0), Is.EqualTo(unchecked((ulong)0xff00cc008000dd00)));
      Assert.That(BinaryConversion.ToUInt64LE(new byte[] {0xff, 0x00, 0xcc, 0x00, 0x80, 0x00, 0xdd, 0x00}, 0), Is.EqualTo(unchecked((ulong)0x00dd008000cc00ff)));

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.ToUInt64(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.ToUInt64(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}, 1, asLittleEndian: false));
    }

    [Test]
    public void TestGetBytesInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0xff, 0x00, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x00, 0xff, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false), Is.EqualTo(new byte[] {0xff, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: true), Is.EqualTo(new byte[] {0x00, 0xff}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(short.MinValue, asLittleEndian: false), Is.EqualTo(new byte[] {0x80, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(short.MinValue, asLittleEndian: true), Is.EqualTo(new byte[] {0x00, 0x80}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(short.MinValue), Is.EqualTo(new byte[] {0x80, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(short.MinValue), Is.EqualTo(new byte[] {0x00, 0x80}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false, new byte[1], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false, new byte[1], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((short)0xff00), asLittleEndian: false, new byte[2], 1));
    }

    [Test]
    public void TestGetBytesUInt16()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0xff, 0x00, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x00, 0xff, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false), Is.EqualTo(new byte[] {0xff, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: true), Is.EqualTo(new byte[] {0x00, 0xff}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(unchecked((ushort)0xff00)), Is.EqualTo(new byte[] {0xff, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(unchecked((ushort)0xff00)), Is.EqualTo(new byte[] {0x00, 0xff}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false, new byte[1], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false, new byte[1], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ushort)0xff00), asLittleEndian: false, new byte[2], 1));
    }

    [Test]
    public void TestGetBytesInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: true), Is.EqualTo(new byte[] {0x44, 0x33, 0x22, 0x11}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(int.MinValue, asLittleEndian: false), Is.EqualTo(new byte[] {0x80, 0x00, 0x00, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(int.MinValue, asLittleEndian: true), Is.EqualTo(new byte[] {0x00, 0x00, 0x00, 0x80}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(int.MinValue), Is.EqualTo(new byte[] {0x80, 0x00, 0x00, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(int.MinValue), Is.EqualTo(new byte[] {0x00, 0x00, 0x00, 0x80}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false, new byte[3], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false, new byte[3], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((int)0x11223344), asLittleEndian: false, new byte[4], 1));
    }

    [Test]
    public void TestGetBytesUInt32()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x44, 0x33, 0x22, 0x11, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: true), Is.EqualTo(new byte[] {0x44, 0x33, 0x22, 0x11}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(unchecked((uint)0x11223344)), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(unchecked((uint)0x11223344)), Is.EqualTo(new byte[] {0x44, 0x33, 0x22, 0x11}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false, new byte[3], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false, new byte[3], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((uint)0x11223344), asLittleEndian: false, new byte[4], 1));
    }

    [Test]
    public void TestGetBytesInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: true), Is.EqualTo(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(long.MinValue, asLittleEndian: false), Is.EqualTo(new byte[] {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(long.MinValue, asLittleEndian: true), Is.EqualTo(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(long.MinValue), Is.EqualTo(new byte[] {0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(long.MinValue), Is.EqualTo(new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false, new byte[7], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false, new byte[7], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((long)0x1122334455667788), asLittleEndian: false, new byte[8], 1));
    }

    [Test]
    public void TestGetBytesUInt64()
    {
      var buffer = new byte[] {0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc};

      BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xcc}).AsCollection);

      BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] {0xdd, 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc}).AsCollection);

      Assert.That(BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88}).AsCollection);
      Assert.That(BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: true), Is.EqualTo(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11}).AsCollection);
      Assert.That(BinaryConversion.GetBytesBE(unchecked((ulong)0x1122334455667788)), Is.EqualTo(new byte[] {0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88}).AsCollection);
      Assert.That(BinaryConversion.GetBytesLE(unchecked((ulong)0x1122334455667788)), Is.EqualTo(new byte[] {0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11}).AsCollection);

      Assert.Throws<ArgumentNullException>      (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false, new byte[7], -1));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false, new byte[7], 0));
      Assert.Throws<ArgumentException>          (() => BinaryConversion.GetBytes(unchecked((ulong)0x1122334455667788), asLittleEndian: false, new byte[8], 1));
    }

    [Test]
    public void TestToUInt24()
    {
      Assert.That(BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, asLittleEndian: false), Is.EqualTo((UInt24)0xff0000));
      Assert.That(BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, asLittleEndian: false), Is.EqualTo((UInt24)0x0000cc));
      Assert.That(BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00 }, 0, asLittleEndian: true), Is.EqualTo((UInt24)0x0000ff));
      Assert.That(BinaryConversion.ToUInt24(new byte[] { 0xff, 0x00, 0x00, 0xcc }, 1, asLittleEndian: true), Is.EqualTo((UInt24)0xcc0000));

      Assert.Throws<ArgumentNullException>(() => BinaryConversion.ToUInt24(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00 }, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00 }, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>(() => BinaryConversion.ToUInt24(new byte[] { 0x00, 0x00, 0x00 }, 1, asLittleEndian: false));
    }

    [Test]
    public void TestToUInt48()
    {
      Assert.That(BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, asLittleEndian: false), Is.EqualTo((UInt48)0xff0000000000));
      Assert.That(BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, asLittleEndian: false), Is.EqualTo((UInt48)0x0000000000cc));
      Assert.That(BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, asLittleEndian: true), Is.EqualTo((UInt48)0x0000000000ff));
      Assert.That(BinaryConversion.ToUInt48(new byte[] { 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0xcc }, 1, asLittleEndian: true), Is.EqualTo((UInt48)0xcc0000000000));

      Assert.Throws<ArgumentNullException>(() => BinaryConversion.ToUInt48(null, 0, asLittleEndian: false));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, -1, asLittleEndian: false));
      Assert.Throws<ArgumentException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, asLittleEndian: false));
      Assert.Throws<ArgumentException>(() => BinaryConversion.ToUInt48(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 1, asLittleEndian: false));
    }

    [Test]
    public void TestGetBytesUInt24()
    {
      var buffer = new byte[] { 0xdd, 0xcc, 0xdd, 0xcc, 0xdd };

      BinaryConversion.GetBytes((UInt24)0x112233, asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0xdd, 0x11, 0x22, 0x33, 0xdd }).AsCollection);

      BinaryConversion.GetBytes((UInt24)0x112233, asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0xdd, 0x33, 0x22, 0x11, 0xdd }).AsCollection);

      Assert.That(BinaryConversion.GetBytes((UInt24)0x112233, asLittleEndian: false), Is.EqualTo(new byte[] { 0x11, 0x22, 0x33 }).AsCollection);
      Assert.That(BinaryConversion.GetBytes((UInt24)0x112233, asLittleEndian: true), Is.EqualTo(new byte[] { 0x33, 0x22, 0x11 }).AsCollection);

      Assert.Throws<ArgumentNullException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), asLittleEndian: false, new byte[2], -1));
      Assert.Throws<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), asLittleEndian: false, new byte[2], 0));
      Assert.Throws<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt24)0x112233), asLittleEndian: false, new byte[3], 1));
    }

    [Test]
    public void TestGetBytesUInt48()
    {
      var buffer = new byte[] { 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc, 0xdd, 0xcc };

      BinaryConversion.GetBytes((UInt48)0x112233445566, asLittleEndian: false, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0xdd, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0xcc }).AsCollection);

      BinaryConversion.GetBytes((UInt48)0x112233445566, asLittleEndian: true, buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0xdd, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11, 0xcc }).AsCollection);

      Assert.That(BinaryConversion.GetBytes((UInt48)0x112233445566, asLittleEndian: false), Is.EqualTo(new byte[] { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66 }).AsCollection);
      Assert.That(BinaryConversion.GetBytes((UInt48)0x112233445566, asLittleEndian: true), Is.EqualTo(new byte[] { 0x66, 0x55, 0x44, 0x33, 0x22, 0x11 }).AsCollection);

      Assert.Throws<ArgumentNullException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), asLittleEndian: false, null, 0));
      Assert.Throws<ArgumentOutOfRangeException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), asLittleEndian: false, new byte[5], -1));
      Assert.Throws<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), asLittleEndian: false, new byte[5], 0));
      Assert.Throws<ArgumentException>(() => BinaryConversion.GetBytes(unchecked((UInt48)0x112233445566), asLittleEndian: false, new byte[6], 1));
    }
  }
}

