using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class BigEndianBinaryReaderTest {
    [Test]
    public void TestReadUnsignedInt()
    {
      var stream = new MemoryStream(new byte[] {
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00,
        0x00,
      });

      var reader = new BigEndianBinaryReader(stream);

      Assert.AreEqual((ulong)0x8000000000000000, reader.ReadUInt64());
      Assert.AreEqual((uint)0x80000000, reader.ReadUInt32());
      Assert.AreEqual((byte)0x80, reader.ReadByte());
      Assert.AreEqual((ushort)0x8000, reader.ReadUInt16());
      Assert.AreEqual((byte)0x00, reader.ReadByte());
      Assert.AreEqual((UInt48)0x800000000000, reader.ReadUInt48());
      Assert.AreEqual((UInt24)0x800000, reader.ReadUInt24());

      Assert.AreEqual(stream.Length, reader.BaseStream.Position, "position");
    }

    [Test]
    public void TestReadSignedInt()
    {
      var stream = new MemoryStream(new byte[] {
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00,
        0x00,
      });

      var reader = new BigEndianBinaryReader(stream);

      Assert.AreEqual(long.MinValue, reader.ReadInt64());
      Assert.AreEqual(int.MinValue, reader.ReadInt32());
      Assert.AreEqual(sbyte.MinValue, reader.ReadSByte());
      Assert.AreEqual(short.MinValue, reader.ReadInt16());
      Assert.AreEqual((sbyte)0, reader.ReadSByte());
      Assert.AreEqual((UInt48)0x800000000000, reader.ReadUInt48());
      Assert.AreEqual((UInt24)0x800000, reader.ReadUInt24());

      Assert.AreEqual(stream.Length, reader.BaseStream.Position, "position");
    }

    [Test]
    public void TestReadFourCC()
    {
      var reader = new BigEndianBinaryReader(new MemoryStream(new byte[] {
        0x52, 0x49, 0x46, 0x46,
      }));

      Assert.AreEqual("RIFF", reader.ReadFourCC().ToString());
    }
  }
}
