//
// Copyright (c) 2010 smdn <smdn@smdn.jp>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Text.RegularExpressions;

using Smdn.IO;
using Smdn.IO.Streams.LineOriented;
using Smdn.Security.Cryptography;
using Smdn.Text;

namespace Smdn.Formats.UUEncodings {
  public class UUDecodingStream : Stream {
    private enum State {
      Initial,
      DataLine,
      EndOfFile,
      EndOfStream,
    }

    public override bool CanSeek {
      get { return false; }
    }

    public override bool CanRead {
      get { return !IsClosed; }
    }

    public override bool CanWrite {
      get { return false; }
    }

    public override bool CanTimeout {
      get { return !IsClosed && stream.CanTimeout; }
    }

    private bool IsClosed {
      get { return stream == null; }
    }

    public override long Position {
      get { throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
      set { throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
    }

    public override long Length {
      get { throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
    }

    public string FileName {
      get {
        CheckDisposed();

        if (state == State.Initial)
          InternalSeekToNextFile();

        return fileName;
      }
    }

    [CLSCompliant(false)]
    public uint Permissions {
      get {
        CheckDisposed();

        if (state == State.Initial)
          InternalSeekToNextFile();

        return permissions;
      }
    }

    public bool EndOfFile {
      get { CheckDisposed(); return state == State.EndOfFile || state == State.EndOfStream; }
    }

    public UUDecodingStream(Stream baseStream)
      : this(baseStream, false)
    {
    }

    public UUDecodingStream(Stream baseStream, bool leaveStreamOpen)
    {
      if (baseStream == null)
        throw new ArgumentNullException(nameof(baseStream));
      if (!baseStream.CanRead)
        throw ExceptionUtils.CreateArgumentMustBeReadableStream(nameof(baseStream));

      this.leaveStreamOpen = leaveStreamOpen;
      this.stream = new LooseLineOrientedStream(baseStream, leaveStreamOpen);
      this.transform = new UUDecodingTransform();
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (stream != null) {
        if (!leaveStreamOpen)
          stream.Close();

        stream = null;
      }

      if (transform != null) {
        transform.Clear();
        transform = null;
      }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      base.Close();
#else
      base.Dispose(disposing);
#endif
    }

    public bool SeekToNextFile()
    {
      CheckDisposed();

      InternalSeekToNextFile();

      return (state == State.DataLine);
    }

    private static readonly Regex regexHeaderLine = new Regex(@"^begin\s+(?<perms>[0-7]{3,4})\s+(?<filename>.*)$", RegexOptions.Singleline);

    private void InternalSeekToNextFile()
    {
      permissions = 0u;
      fileName = null;

      if (state == State.EndOfStream)
        return;

      for (;;) {
        var l = stream.ReadLine(true);

        if (l == null) {
          state = State.EndOfStream;
          break;
        }

        var line = ByteString.CreateImmutable(l);

        if (line.StartsWith("begin ")) {
          var match = regexHeaderLine.Match(line.ToString().TrimEnd());

          if (match.Success) {
            permissions = Convert.ToUInt32(match.Groups["perms"].Value, 16);
            fileName = match.Groups["filename"].Value;
          }

          state = State.DataLine;
          dataLineOffset = 0;
          dataLineRemainder = 0;

          break;
        }
      }
    }

    public override int ReadByte()
    {
      CheckDisposed();

      if (state == State.EndOfFile || state == State.EndOfStream) {
        return -1;
      }
      else if (state == State.DataLine && 0 < dataLineRemainder) {
        var ret = decodedDataLine[dataLineOffset];

        dataLineRemainder--;
        dataLineOffset++;

        return ret;
      }
      else {
        return base.ReadByte();
      }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      if (buffer == null)
        throw new ArgumentNullException(nameof(buffer));
      if (offset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(offset), offset);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (buffer.Length - count < offset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(offset), buffer, offset, count);

      if (count == 0)
        return 0;

      if (state == State.Initial)
        InternalSeekToNextFile();

      if (state == State.EndOfFile || state == State.EndOfStream)
        return 0;

      var ret = 0;

      for (;;) {
        if (dataLineRemainder == 0) {
          var dataLine = stream.ReadLine(true);

          if (dataLine == null) {
            dataLineRemainder = 0;
            state = State.EndOfStream;
            break;
          }
          else if (3 <= dataLine.Length &&
                   dataLine[0] == 0x65 /* 'e' */ &&
                   dataLine[1] == 0x6e /* 'n' */ &&
                   dataLine[2] == 0x64 /* 'd' */) {
            /*
             * footer line
             */
            dataLine = null;
            dataLineRemainder = 0;
            state = State.EndOfFile;
            break;
          }
          else {
            decodedDataLine = transform.TransformBytes(dataLine, 0, dataLine.Length);
            dataLineOffset = 0;
            dataLineRemainder = decodedDataLine.Length;
          }
        }

        var bytesToCopy = Math.Min(dataLineRemainder, count);

        Buffer.BlockCopy(decodedDataLine, dataLineOffset, buffer, offset, bytesToCopy);

        dataLineOffset += bytesToCopy;
        dataLineRemainder -= bytesToCopy;

        offset += bytesToCopy;
        count -= bytesToCopy;
        ret += bytesToCopy;

        if (count <= 0)
          break;
      }

      return ret;
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedSettingStreamLength();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedSeekingStream();
    }

    public override void Flush()
    {
      CheckDisposed();

      // do nothing
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedWritingStream();
    }

    private void CheckDisposed()
    {
      if (IsClosed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private readonly bool leaveStreamOpen;
    private LineOrientedStream stream;
    private State state = State.Initial;
    private uint permissions;
    private string fileName;
    private UUDecodingTransform transform;
    private byte[] decodedDataLine;
    private int dataLineOffset;
    private int dataLineRemainder;
  }
}

