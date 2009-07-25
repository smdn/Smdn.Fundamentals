using System;
using System.IO;
using NUnit.Framework;

using Smdn.Extensions;

namespace Smdn.IO {
  [TestFixture]
  public class BinaryWriterTest {
    [Test]
    public void TestWriteZero()
    {
      Predicate<byte> allzero = delegate(byte b) {
        return b == 0x00;
      };

      foreach (var len in new[] {
        0,
        3,
        7,
        8,
        12,
      }) {
        byte[] arr;

        using (var stream = new MemoryStream(0x10)) {
          var writer = new BinaryWriter(stream);

          writer.WriteZero(len);
          writer.Flush();

          Assert.AreEqual(len, writer.BaseStream.Position);

          writer.Close();
          stream.Close();

          arr = stream.ToArray();

          Assert.AreEqual(len, arr.Length);
          Assert.IsTrue(Array.TrueForAll(arr, allzero));
        }
      }
    }
  }
}
