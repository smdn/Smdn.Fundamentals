using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO.Binary {
  [TestFixture]
  public class BigEndianBinaryWriterTests {
    [Test]
    public void TestWriteUnsignedInt()
    {
      var stream = new MemoryStream();
      var writer = new BigEndianBinaryWriter(stream);

      writer.Write((ulong)0x8000000000000000);
      writer.Write((uint)0x80000000);
      writer.Write((byte)0x80);
      writer.Write((ushort)0x8000);
      writer.Write((byte)0x00);
      writer.Write((UInt48)0x800000000000);
      writer.Write((UInt24)0x800000);

      var actual = new byte[] {
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00,
        0x00,
      };

      Assert.AreEqual(stream.Position, actual.Length, "length");
      Assert.AreEqual(stream.ToArray(), actual);
    }

    [Test]
    public void TestWriteSignedInt()
    {
      var stream = new MemoryStream();
      var writer = new BigEndianBinaryWriter(stream);

      writer.Write(long.MinValue);
      writer.Write(int.MinValue);
      writer.Write(sbyte.MinValue);
      writer.Write(short.MinValue);
      writer.Write((sbyte)0);
      writer.Write((UInt48)0x800000000000);
      writer.Write((UInt24)0x800000);

      var actual = new byte[] {
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x80, 0x80, 0x00, 0x00,
        0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00,
        0x00,
      };

      Assert.AreEqual(stream.Position, actual.Length, "length");
      Assert.AreEqual(stream.ToArray(), actual);
    }

    [Test]
    public void TestWriteFourCC()
    {
      var stream = new MemoryStream();
      var writer = new BigEndianBinaryWriter(stream);

      writer.Write(new FourCC("RIFF"));

      var actual = new byte[] {
        0x52, 0x49, 0x46, 0x46,
      };

      Assert.AreEqual(stream.Position, actual.Length, "length");
      Assert.AreEqual(stream.ToArray(), actual);
    }
  }
}
