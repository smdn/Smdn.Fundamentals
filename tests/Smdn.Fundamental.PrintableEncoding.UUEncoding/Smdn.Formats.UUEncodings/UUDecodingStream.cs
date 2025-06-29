// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using NUnit.Framework;

using Smdn.IO.Streams;

namespace Smdn.Formats.UUEncodings {
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
        Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0644", 8)), "Permissions");
        Assert.That(stream.FileName, Is.EqualTo("cat.txt"), "FileName");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile");

        Assert.That(stream.EndOfFile, Is.False, "EndOfFile");
        Assert.That(stream.CanRead, Is.True, "CanRead");
        Assert.That(stream.CanWrite, Is.False, "CanWrite");
        Assert.That(stream.CanSeek, Is.False, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

        Assert.Throws<NotSupportedException>(() => Assert.That(stream.Length, Is.EqualTo(-1)), "Length");
        Assert.Throws<NotSupportedException>(() => Assert.That(stream.Position, Is.EqualTo(-1)), "Position");

        stream.Dispose();

        Assert.That(stream.CanRead, Is.False, "CanRead");
        Assert.That(stream.CanWrite, Is.False, "CanWrite");
        Assert.That(stream.CanSeek, Is.False, "CanSeek");
        Assert.That(stream.CanTimeout, Is.False, "CanTimeout");

        Assert.Throws<NotSupportedException>(() => Assert.That(stream.Length, Is.EqualTo(-1)), "Length");
        Assert.Throws<NotSupportedException>(() => Assert.That(stream.Position, Is.EqualTo(-1)), "Position");
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
        Assert.Throws<NotSupportedException>(() => stream.Seek(0L, SeekOrigin.Begin));
        Assert.Throws<NotSupportedException>(() => stream.SetLength(0L));

        stream.Dispose();

        Assert.Throws<ObjectDisposedException>(() => stream.Write(new byte[] {0x00}, 0, 1));
        Assert.Throws<ObjectDisposedException>(() => stream.WriteByte((byte)0x00));
        Assert.Throws<ObjectDisposedException>(() => stream.Seek(0L, SeekOrigin.Begin));
        Assert.Throws<ObjectDisposedException>(() => stream.SetLength(0L));
      }
    }

    [Test]
    public void TestFlush()
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";

      using (var baseStream = CreateStream(input)) {
        using (var stream = new UUDecodingStream(baseStream)) {
          Assert.DoesNotThrow(() => stream.Flush());
        }
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

          Assert.DoesNotThrow(() => _ = stream.Read(buffer, 0, 1), "Read");
          Assert.DoesNotThrow(() => stream.ReadByte(), "ReadByte");

          stream.Dispose();

          Assert.Throws<ObjectDisposedException>(() => _ = stream.Read(buffer, 0, 1), "Read");
          Assert.Throws<ObjectDisposedException>(() => stream.ReadByte(), "ReadByte");
          Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0644", 8))), "Permissions");
          Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.FileName, Is.EqualTo("cat.txt")), "FileName");
          Assert.Throws<ObjectDisposedException>(() => Assert.That(stream.EndOfFile, Is.True), "EndOfFile");

          Assert.Throws<ObjectDisposedException>(() => baseStream.ReadByte(), "baseStream.ReadByte()");

          Assert.DoesNotThrow(() => stream.Dispose(), "Dispose() again");
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
          stream.Dispose();

          Assert.DoesNotThrow(() => baseStream.ReadByte(), "baseStream.ReadByte()");

          Assert.DoesNotThrow(() => stream.Dispose(), "Dispose() again");
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
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile initial");

        Assert.That(stream.SeekToNextFile(), Is.True, "SeekToNextFile cat1.txt");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile cat1.txt");
        Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0001", 8)), "Permissions cat1.txt");
        Assert.That(stream.FileName, Is.EqualTo("cat1.txt"), "FileName cat1.txt");
        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'C'), "ReadByte cat1.txt");

        Assert.That(stream.SeekToNextFile(), Is.True, "SeekToNextFile cat2.txt");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile cat2.txt");
        Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0002", 8)), "Permissions cat2.txt");
        Assert.That(stream.FileName, Is.EqualTo("cat2.txt"), "FileName cat2.txt");
        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'C'), "ReadByte cat2.txt");

        var buffer = new byte[5];

        Assert.That(stream.Read(buffer, 0, buffer.Length), Is.EqualTo(2), "Read cat2.txt");
        Assert.That(stream.EndOfFile, Is.True, "EndOfFile cat2.txt");

        Assert.That(stream.SeekToNextFile(), Is.True, "SeekToNextFile cat3.txt");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile cat3.txt");
        Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0003", 8)), "Permissions cat3.txt");
        Assert.That(stream.FileName, Is.EqualTo("cat3.txt"), "FileName cat3.txt");
        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'C'), "ReadByte cat3.txt");

        Assert.That(stream.SeekToNextFile(), Is.False, "SeekToNextFile final");
        Assert.That(stream.EndOfFile, Is.True, "EndOfFile final");

        Assert.That(stream.SeekToNextFile(), Is.False, "SeekToNextFile final again");
        Assert.That(stream.EndOfFile, Is.True, "EndOfFile final again");
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
        Assert.That(stream.EndOfFile, Is.False, "#1");
        Assert.That(stream.SeekToNextFile(), Is.False, "#2");
        Assert.That(stream.EndOfFile, Is.True, "#3");
        Assert.That(stream.SeekToNextFile(), Is.False, "#4");
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
          new {Permissions = Convert.ToUInt32("0001", 8), FileName = "1.txt"},
          new {Permissions = Convert.ToUInt32("10000", 8), FileName = "2.txt"},
          new {Permissions = Convert.ToUInt32("0000", 8), FileName = "3.txt"},
          new {Permissions = Convert.ToUInt32("0777", 8), FileName = "4.txt"},
          new {Permissions = Convert.ToUInt32("6745", 8), FileName = "5.txt"},
        }) {
          Assert.That(stream.Permissions, Is.EqualTo(expected.Permissions), $"Permissions #{index}");
          Assert.That(stream.FileName, Is.EqualTo(expected.FileName), $"FileName #{index}");

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
        Assert.That(stream.Permissions, Is.EqualTo(Convert.ToUInt32("0644", 8)));
        Assert.That(stream.FileName, Is.EqualTo("cat.txt"));

        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'C'), "C");
        Assert.That(stream.EndOfFile, Is.False, "C");

        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'a'), "a");
        Assert.That(stream.EndOfFile, Is.False, "C");

        Assert.That(stream.ReadByte(), Is.EqualTo((byte)'t'), "t");
        Assert.That(stream.EndOfFile, Is.False, "C");

        Assert.That(stream.ReadByte(), Is.EqualTo(-1), "end of file");
        Assert.That(stream.EndOfFile, Is.True, "end of file");
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

        Assert.That(stream.Read(buffer, 0, 1), Is.EqualTo(1), "Read #1");
        Assert.That(buffer[0], Is.EqualTo((byte)'C'), "#1");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile #1");

        Assert.That(stream.Read(buffer, 1, 1), Is.EqualTo(1), "Read #2");
        Assert.That(buffer[1], Is.EqualTo((byte)'a'), "#2");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile #2");

        Assert.That(stream.Read(buffer, 2, 1), Is.EqualTo(1), "Read #3");
        Assert.That(buffer[2], Is.EqualTo((byte)'t'), "#3");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile #3");

        Assert.That(stream.Read(buffer, 0, 2), Is.EqualTo(2), "Read #4");
        Assert.That(buffer[0], Is.EqualTo((byte)'C'), "#4-1");
        Assert.That(buffer[1], Is.EqualTo((byte)'a'), "#4-2");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile #4");

        Assert.That(stream.Read(buffer, 0, 2), Is.EqualTo(2), "Read #5");
        Assert.That(buffer[0], Is.EqualTo((byte)'t'), "#5-1");
        Assert.That(buffer[1], Is.EqualTo((byte)'C'), "#5-2");
        Assert.That(stream.EndOfFile, Is.False, "EndOfFile #5");

        Assert.That(stream.Read(buffer, 0, 5), Is.EqualTo(2), "Read #6");
        Assert.That(buffer[0], Is.EqualTo((byte)'a'), "#5-1");
        Assert.That(buffer[1], Is.EqualTo((byte)'t'), "#5-2");
        Assert.That(stream.EndOfFile, Is.True, "EndOfFile #5");
      }
    }

    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestRead_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      var input = @"begin 644 cat.txt
#0V%T
`
end";
      using var stream = new UUDecodingStream(CreateStream(input));

      Assert.Throws(expectedExceptionType, () => _ = stream.Read(buffer, offset, count));
    }

#if false
    [TestCaseSource(
      typeof(StreamTestCaseSource),
      nameof(StreamTestCaseSource.YieldTestCases_InvalidReadBufferArguments)
    )]
    public void TestReadAsync_InvalidBufferArguments(
      byte[] buffer,
      int offset,
      int count,
      Type expectedExceptionType
    )
    {
      Assert.Throws(expectedExceptionType, () => stream.ReadAsync(buffer, offset, count));
    }
#endif
  }
}

