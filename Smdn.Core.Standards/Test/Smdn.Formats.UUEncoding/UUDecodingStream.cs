using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats.UUEncoding {
  [TestFixture]
  public class UUDecodingStreamTests {
    private MemoryStream CreateStream(string input)
    {
      return new MemoryStream(Encoding.ASCII.GetBytes(input));
    }

    [Test]
    public void TestProperties()
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        Assert.AreEqual(0x0644, stream.Permissions, "Permissions");
        Assert.AreEqual("cat.txt", stream.FileName, "FileName");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile");

        Assert.IsFalse(stream.EndOfFile, "EndOfFile");
        Assert.IsTrue(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        Assert.Throws<NotSupportedException>(() => Assert.AreEqual(-1, stream.Length), "Length");
        Assert.Throws<NotSupportedException>(() => Assert.AreEqual(-1, stream.Position), "Position");

        stream.Close();

        Assert.IsFalse(stream.CanRead, "CanRead");
        Assert.IsFalse(stream.CanWrite, "CanWrite");
        Assert.IsFalse(stream.CanSeek, "CanSeek");
        Assert.IsFalse(stream.CanTimeout, "CanTimeout");

        Assert.Throws<NotSupportedException>(() => Assert.AreEqual(-1, stream.Length), "Length");
        Assert.Throws<NotSupportedException>(() => Assert.AreEqual(-1, stream.Position), "Position");
      }
    }

    [Test]
    public void TestUnsupportedOperations()
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] {0x00}, 0, 1));
        Assert.Throws<NotSupportedException>(() => stream.WriteByte((byte)0x00));
        Assert.Throws<NotSupportedException>(() => stream.Flush());
        Assert.Throws<NotSupportedException>(() => stream.Seek(0L, SeekOrigin.Begin));
        Assert.Throws<NotSupportedException>(() => stream.SetLength(0L));

        stream.Close();

        Assert.Throws<ObjectDisposedException>(() => stream.Write(new byte[] {0x00}, 0, 1));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte((byte)0x00));
        Assert.Throws<ObjectDisposedException>(() => stream.Flush());
        Assert.Throws<ObjectDisposedException>(() => stream.Seek(0L, SeekOrigin.Begin));
        Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L));
      }
    }

    [Test]
    public void TestClose()
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";

      using (var baseStream = CreateStream(input)) {
        using (var stream = new UUDecodingStream(baseStream)) {
          var buffer = new byte[3];

          Assert.DoesNotThrow(() => stream.Read(buffer, 0, 1), "Read");
          Assert.DoesNotThrow(() => stream.ReadByte(), "ReadByte");

          stream.Close();

          Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, 1), "Read");
          Assert.Throws<ObjectDisposedException>(() => stream.ReadByte(), "ReadByte");
          Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual(0x0644, stream.Permissions), "Permissions");
          Assert.Throws<ObjectDisposedException>(() => Assert.AreEqual("cat.txt", stream.FileName), "FileName");
          Assert.Throws<ObjectDisposedException>(() => Assert.IsTrue(stream.EndOfFile), "EndOfFile");

          Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte(), "baseStream.ReadByte()");

          Assert.DoesNotThrow(() => stream.Close(), "Close() again");
        }
      }
    }

    [Test]
    public void TestCloseLeaveStreamOpen()
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";

      using (var baseStream = CreateStream(input)) {
        using (var stream = new UUDecodingStream(baseStream, true)) {
          stream.Close();

          Assert.DoesNotThrow(() => baseStream.ReadByte(), "baseStream.ReadByte()");

          Assert.DoesNotThrow(() => stream.Close(), "Close() again");
        }
      }
    }

    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void TestSeekToNextFile(string newline)
    {
      var input = @"

*** extra text block ***
*** extra text block ***

begin 001 cat1.txt
#0V%T
`
end

*** extra text block ***
*** extra text block ***
begin 002 cat2.txt
#0V%T
`
end

begin 003 cat3.txt
#0V%T
`
end
*** extra text block ***
*** extra text block ***

".Replace("\r\n", "\n").Replace("\n", newline);

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        Assert.IsFalse(stream.EndOfFile, "EndOfFile initial");

        Assert.IsTrue(stream.SeekToNextFile(), "SeekToNextFile cat1.txt");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile cat1.txt");
        Assert.AreEqual(0x001, stream.Permissions, "Permissions cat1.txt");
        Assert.AreEqual("cat1.txt", stream.FileName, "FileName cat1.txt");
        Assert.AreEqual((byte)'C', stream.ReadByte(), "ReadByte cat1.txt");

        Assert.IsTrue(stream.SeekToNextFile(), "SeekToNextFile cat2.txt");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile cat2.txt");
        Assert.AreEqual(0x002, stream.Permissions, "Permissions cat2.txt");
        Assert.AreEqual("cat2.txt", stream.FileName, "FileName cat2.txt");
        Assert.AreEqual((byte)'C', stream.ReadByte(), "ReadByte cat2.txt");

        var buffer = new byte[5];

        Assert.AreEqual(2, stream.Read(buffer, 0, buffer.Length), "Read cat2.txt");
        Assert.IsTrue(stream.EndOfFile, "EndOfFile cat2.txt");

        Assert.IsTrue(stream.SeekToNextFile(), "SeekToNextFile cat3.txt");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile cat3.txt");
        Assert.AreEqual(0x003, stream.Permissions, "Permissions cat3.txt");
        Assert.AreEqual("cat3.txt", stream.FileName, "FileName cat3.txt");
        Assert.AreEqual((byte)'C', stream.ReadByte(), "ReadByte cat3.txt");

        Assert.IsFalse(stream.SeekToNextFile(), "SeekToNextFile final");
        Assert.IsTrue(stream.EndOfFile, "EndOfFile final");

        Assert.IsFalse(stream.SeekToNextFile(), "SeekToNextFile final again");
        Assert.IsTrue(stream.EndOfFile, "EndOfFile final again");
      }
    }

    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void TestSeekToNextFileContainsNoUUEncoded(string newline)
    {
      var input = @"

*** extra text block ***

begin
#0V%T
`
end

*** extra text block ***

".Replace("\r\n", "\n").Replace("\n", newline);

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        Assert.IsFalse(stream.EndOfFile, "#1");
        Assert.IsFalse(stream.SeekToNextFile(), "#2");
        Assert.IsTrue(stream.EndOfFile, "#3");
        Assert.IsFalse(stream.SeekToNextFile(), "#4");
      }
    }

    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void TestFileNameAndPermissions(string newline)
    {
      var input = @"

begin 1 1.txt
#0V%T
`
end

begin 10000 2.txt                  
#0V%T
`
end

begin 888      3.txt
#0V%T
`
end

begin 777      4.txt            
#0V%T
`
end

begin 6745 5.txt
#0V%T
`
end

".Replace("\r\n", "\n").Replace("\n", newline);

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        var index = 0;

        foreach (var expected in new[] {
          new {Permissions = 0u, FileName = (string)null},
          new {Permissions = 0u, FileName = (string)null},
          new {Permissions = 0u, FileName = (string)null},
          new {Permissions = (uint)0x0777, FileName = "4.txt"},
          new {Permissions = (uint)0x6745, FileName = "5.txt"},
        }) {
          Assert.AreEqual(expected.Permissions, stream.Permissions, "Permissions #{0}", index);
          Assert.AreEqual(expected.FileName, stream.FileName, "FileName #{0}", index);

          stream.SeekToNextFile();

          index++;
        }
      }
    }

    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void TestReadByte(string newline)
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end".Replace("\r\n", "\n").Replace("\n", newline);

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        Assert.AreEqual(0x0644, stream.Permissions);
        Assert.AreEqual("cat.txt", stream.FileName);

        Assert.AreEqual((byte)'C', stream.ReadByte(), "C");
        Assert.IsFalse(stream.EndOfFile, "C");

        Assert.AreEqual((byte)'a', stream.ReadByte(), "a");
        Assert.IsFalse(stream.EndOfFile, "C");

        Assert.AreEqual((byte)'t', stream.ReadByte(), "t");
        Assert.IsFalse(stream.EndOfFile, "C");

        Assert.AreEqual(-1, stream.ReadByte(), "end of file");
        Assert.IsTrue(stream.EndOfFile, "end of file");
      }
    }

    [TestCase("\r")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void TestRead(string newline)
    {
      var input = @"*** extra text block ***
begin 644 cat.txt
#0V%T
#0V%T
#0V%T
`
end
*** extra text block ***
".Replace("\r\n", "\n").Replace("\n", newline);

      using (var stream = new UUDecodingStream(CreateStream(input))) {
        var buffer = new byte[5];

        Assert.AreEqual(1, stream.Read(buffer, 0, 1), "Read #1");
        Assert.AreEqual((byte)'C', buffer[0], "#1");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile #1");

        Assert.AreEqual(1, stream.Read(buffer, 1, 1), "Read #2");
        Assert.AreEqual((byte)'a', buffer[1], "#2");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile #2");

        Assert.AreEqual(1, stream.Read(buffer, 2, 1), "Read #3");
        Assert.AreEqual((byte)'t', buffer[2], "#3");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile #3");

        Assert.AreEqual(2, stream.Read(buffer, 0, 2), "Read #4");
        Assert.AreEqual((byte)'C', buffer[0], "#4-1");
        Assert.AreEqual((byte)'a', buffer[1], "#4-2");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile #4");

        Assert.AreEqual(2, stream.Read(buffer, 0, 2), "Read #5");
        Assert.AreEqual((byte)'t', buffer[0], "#5-1");
        Assert.AreEqual((byte)'C', buffer[1], "#5-2");
        Assert.IsFalse(stream.EndOfFile, "EndOfFile #5");

        Assert.AreEqual(2, stream.Read(buffer, 0, 5), "Read #6");
        Assert.AreEqual((byte)'a', buffer[0], "#5-1");
        Assert.AreEqual((byte)'t', buffer[1], "#5-2");
        Assert.IsTrue(stream.EndOfFile, "EndOfFile #5");
      }
    }
  }
}

