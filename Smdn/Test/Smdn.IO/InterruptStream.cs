using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class InterruptStreamTests {
    [Test]
    public void TestTransparency()
    {
      var innerStream = new MemoryStream(8);
      var stream = new InterruptStream(innerStream);

      innerStream.Write(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, 0, 8);

      Assert.AreEqual(innerStream.CanRead, stream.CanRead, "CanRead");
      Assert.AreEqual(innerStream.CanWrite, stream.CanWrite, "CanWrite");
      Assert.AreEqual(innerStream.CanSeek, stream.CanSeek, "CanSeek");
      Assert.AreEqual(innerStream.CanTimeout, stream.CanTimeout, "CanTimeout");
      Assert.AreEqual(innerStream.Length, stream.Length, "Length");

      stream.Position = 3;

      Assert.AreEqual(3, stream.Position, "stream.Position");
      Assert.AreEqual(3, innerStream.Position, "innerStream.Position");

      stream.Seek(0, SeekOrigin.Begin);

      Assert.AreEqual(0, stream.Position, "seeked stream.Position");
      Assert.AreEqual(0, innerStream.Position, "seeked innerStream.Position");

      stream.SetLength(16);

      Assert.AreEqual(16, stream.Length, "after set stream.Length");
      Assert.AreEqual(16, innerStream.Length, "after set innerStream.Length");

      stream.Close();

      try {
        innerStream.WriteByte(0x00);
        Assert.Fail("ObjectDisposedException not thrown");
      }
      catch (ObjectDisposedException) {
      }
    }

    private class InterruptContext {
      public string Context;
    }

    private class OnWritingTestStream : InterruptStream {
      public OnWritingTestStream(Stream stream, InterruptContext context)
        : base(stream)
      {
        this.context = context;
      }

      protected override void OnWriting(byte[] src, int offset, int count, out bool abortWrite)
      {
        context.Context = BitConverter.ToString(src, offset, count);

        while (0 <= --count) {
          src[offset++] = 0xff;
        }

        abortWrite = false;
      }

      private InterruptContext context;
    }

    [Test]
    public void TestOnWriting()
    {
      var context = new InterruptContext();
      var stream = new OnWritingTestStream(new MemoryStream(), context);

      stream.Write(new byte[] {0x00, 0x01, 0x02, 0x03}, 1, 2);

      Assert.AreEqual("01-02", context.Context);

      stream.Close();

      Assert.AreEqual(new byte[] {0xff, 0xff}, (stream.InnerStream as MemoryStream).ToArray());
    }

    [Test]
    public void TestOnWritten()
    {
    }

    [Test]
    public void TestOnReading()
    {
    }

    private class OnReadTestStream : InterruptStream {
      public OnReadTestStream(Stream stream, InterruptContext context)
        : base(stream)
      {
        this.context = context;
      }

      protected override int OnRead(byte[] dest, int offset, int count)
      {
        context.Context  = string.Format("{0},", count);
        context.Context += BitConverter.ToString(dest, offset, count);

        var c = count;

        while (0 <= --c) {
          dest[offset++] = 0xff;
        }

        return count;
      }

      private InterruptContext context;
    }

    [Test]
    public void TestOnRead()
    {
      var context = new InterruptContext();
      var stream = new OnReadTestStream(new MemoryStream(new byte[] {0x00, 0x01, 0x02, 0x03}), context);
      var buffer = new byte[] {0x00, 0x00, 0x00, 0x00};

      Assert.AreEqual(2, stream.Read(buffer, 1, 2));
      Assert.AreEqual("2,00-01", context.Context);
      Console.WriteLine(BitConverter.ToString(buffer));
      Assert.AreEqual(new byte[] {0x00, 0xff, 0xff, 0x00}, buffer);
    }

    [Test]
    public void TestAbortWrite()
    {
      // TODO
    }

    [Test]
    public void TestAbortRead()
    {
      // TODO
    }
  }
}