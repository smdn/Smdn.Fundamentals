// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class LittleEndianBinaryReaderTests {
    [Test]
    public void TestReadUnsignedInt()
    {
      var stream = new MemoryStream(new byte[] {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80,
        0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x80, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00,
        0x80,
      });

      var reader = new LittleEndianBinaryReader(stream);

      Assert.That(reader.ReadUInt64(), Is.EqualTo((ulong)0x8000000000000000));
      Assert.That(reader.ReadUInt32(), Is.EqualTo((uint)0x80000000));
      Assert.That(reader.ReadByte(), Is.EqualTo((byte)0x80));
      Assert.That(reader.ReadUInt16(), Is.EqualTo((ushort)0x8000));
      Assert.That(reader.ReadByte(), Is.Zero);
      Assert.That(reader.ReadUInt48(), Is.EqualTo((UInt48)0x800000000000));
      Assert.That(reader.ReadUInt24(), Is.EqualTo((UInt24)0x800000));

      Assert.That(reader.BaseStream.Position, Is.EqualTo(stream.Length), "position");
    }

    [Test]
    public void TestReadSignedInt()
    {
      var stream = new MemoryStream(new byte[] {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80,
        0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x80, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00,
        0x80,
      });

      var reader = new LittleEndianBinaryReader(stream);

      Assert.That(reader.ReadInt64(), Is.EqualTo(long.MinValue));
      Assert.That(reader.ReadInt32(), Is.EqualTo(int.MinValue));
      Assert.That(reader.ReadSByte(), Is.EqualTo(sbyte.MinValue));
      Assert.That(reader.ReadInt16(), Is.EqualTo(short.MinValue));
      Assert.That(reader.ReadSByte(), Is.Zero);
      Assert.That(reader.ReadUInt48(), Is.EqualTo((UInt48)0x800000000000));
      Assert.That(reader.ReadUInt24(), Is.EqualTo((UInt24)0x800000));

      Assert.That(reader.BaseStream.Position, Is.EqualTo(stream.Length), "position");
    }

    [Test]
    public void TestReadFourCC()
    {
      var reader = new LittleEndianBinaryReader(new MemoryStream(new byte[] {
        0x52, 0x49, 0x46, 0x46,
      }));

      Assert.That(reader.ReadFourCC().ToString(), Is.EqualTo("RIFF"));
    }
  }
}
