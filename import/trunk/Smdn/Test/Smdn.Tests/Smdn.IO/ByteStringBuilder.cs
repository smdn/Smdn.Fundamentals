using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class ByteStringBuilderTests {
    [Test]
    public void TestConstruct()
    {
      var b = new ByteStringBuilder(16);

      Assert.AreEqual(0, b.Length);
      Assert.AreEqual(16, b.Capacity);
      Assert.IsTrue(b.ToByteString().IsEmpty);
    }

    [Test]
    public void TestIndexer()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63});

      Assert.AreEqual(0x61, b[0]);
      Assert.AreEqual(0x62, b[1]);
      Assert.AreEqual(0x63, b[2]);

      b[0] = (byte)0x63;
      b[1] = (byte)0x61;
      b[2] = (byte)0x62;

      Assert.AreEqual(0x63, b[0]);
      Assert.AreEqual(0x61, b[1]);
      Assert.AreEqual(0x62, b[2]);

      try {
#pragma warning disable 168
        var val = b[3];
#pragma warning restore 168
        Assert.Fail("IndexOutOfRangeException not thrown");
      }
      catch (IndexOutOfRangeException) {
      }

      try {
        b[3] = (byte)0x61;
        Assert.Fail("ArgumentOutOfRangeException not thrown");
      }
      catch (ArgumentOutOfRangeException) {
      }
    }

    [Test]
    public void TestAppend()
    {
      var b = new ByteStringBuilder(16);

      Assert.AreEqual(16, b.Capacity);

      Assert.IsTrue(Object.ReferenceEquals(b, b.Append(new ByteString("0123456789"))));

      Assert.AreEqual(10, b.Length);
      Assert.AreEqual(16, b.Capacity);

      Assert.IsTrue(Object.ReferenceEquals(b, b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66})));

      Assert.AreEqual(16, b.Length);
      Assert.AreEqual(16, b.Capacity);

      Assert.IsTrue(Object.ReferenceEquals(b, b.Append((byte)0x67)));

      Assert.IsTrue(16 < b.Capacity);
      Assert.AreEqual(17, b.Length);

      Assert.IsTrue(Object.ReferenceEquals(b, b.Append("xyz")));

      Assert.AreEqual(20, b.Length);

      Assert.AreEqual("0123456789abcdefgxyz", b.ToString());
    }

    [Test]
    public void TestToByteString()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66});

      Assert.AreEqual(new ByteString("abcdef"), b.ToByteString());

      b.Append((byte)0x67);

      Assert.AreEqual(new ByteString("abcdefg"), b.ToByteString());
    }

    [Test]
    public void TestToString()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66});

      Assert.AreEqual("abcdef", b.ToString());

      b.Append((byte)0x67);

      Assert.AreEqual("abcdefg", b.ToString());
    }
  }
}
