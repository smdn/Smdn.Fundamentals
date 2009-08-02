using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class BinaryReaderTest {
    [Test]
    public void TestReadToEnd()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadToEnd());
      }

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        reader.ReadInt16();

        Assert.AreEqual(actual.Slice(2), reader.ReadToEnd());

      }

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        reader.ReadToEnd();

        Assert.AreEqual(new byte[] {}, reader.ReadToEnd());
      }
    }

    [Test]
    public void TestReadBytes()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes((int)reader.BaseStream.Length));
      }

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual, reader.ReadBytes(reader.BaseStream.Length));
      }

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        Assert.AreEqual(actual.Slice(0, 3), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Slice(3, 3), reader.ReadBytes(3L));
        Assert.AreEqual(actual.Slice(6, 2), reader.ReadBytes(3L));
        Assert.AreEqual(new byte[] {}, reader.ReadBytes(3L));
      }
    }

    [Test]
    public void TestReadEndOfStreamException()
    {
      var actual = new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07};

      using (var reader = new BinaryReader(new MemoryStream(actual))) {
        var type = typeof(BinaryReader);

        foreach (var method in new[] {
          type.GetMethod("ReadUInt16BE"),
          type.GetMethod("ReadUInt32BE"),
          type.GetMethod("ReadUInt48BE"),
          type.GetMethod("ReadUInt64BE"),
          type.GetMethod("ReadUInt16LE"),
          type.GetMethod("ReadUInt32LE"),
          type.GetMethod("ReadUInt48LE"),
          type.GetMethod("ReadUInt64LE"),
          type.GetMethod("ReadInt16BE"),
          type.GetMethod("ReadInt32BE"),
          type.GetMethod("ReadInt64BE"),
          type.GetMethod("ReadInt16LE"),
          type.GetMethod("ReadInt32LE"),
          type.GetMethod("ReadInt64LE"),
          type.GetMethod("ReadFourCC"),
        }) {
          reader.BaseStream.Position = reader.BaseStream.Length - 1;

          try {
            method.Invoke(reader, null);
            Assert.Fail("EndOfStreamException not thrown: {0}", method.Name);
          }
          catch (System.Reflection.TargetInvocationException ex) {
            if (!(ex.InnerException is EndOfStreamException))
              Assert.Fail("unexpected exception: {0}", ex);
          }
        }
      }
    }
  }
}
