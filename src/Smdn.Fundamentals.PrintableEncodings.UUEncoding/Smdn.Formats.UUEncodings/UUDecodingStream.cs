// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.IO;
using System.Text; // [net5] EncodingExtensions.GetString(Encoding, ReadOnlySequence<Byte>)
using System.Text.RegularExpressions;

#if !(NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER)
using Smdn.Buffers;
#endif
using Smdn.Formats;
using Smdn.IO.Streams.LineOriented;
using Smdn.Security.Cryptography;
using Smdn.Text;
using Smdn.Text.Encodings;

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
      this.stream = new LooseLineOrientedStream(baseStream, leaveStreamOpen: leaveStreamOpen);
      this.transform = new UUDecodingTransform();
    }

#if SYSTEM_IO_STREAM_CLOSE
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (stream != null) {
        if (!leaveStreamOpen)
#if SYSTEM_IO_STREAM_CLOSE
          stream.Close();
#else
          stream.Dispose();
#endif

        stream = null;
      }

      if (transform != null) {
        transform.Clear();
        transform = null;
      }

#if SYSTEM_IO_STREAM_CLOSE
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

    private static readonly ReadOnlyMemory<byte> headerLinePrefix = new[] { (byte)'b', (byte)'e', (byte)'g', (byte)'i', (byte)'n', (byte)' ' };

    private void InternalSeekToNextFile()
    {
      permissions = 0u;
      fileName = null;

      if (state == State.EndOfStream)
        return;

      for (;;) {
        var l = stream.ReadLine();

        if (!l.HasValue) {
          state = State.EndOfStream;
          break;
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var headerReader = new SequenceReader<byte>(l.Value.Sequence);

        if (headerReader.IsNext(headerLinePrefix.Span, advancePast: true)) {
          const byte SP = 0x20;

          if (!headerReader.TryReadTo(out ReadOnlySequence<byte> mode, SP, advancePastDelimiter: true))
            throw new InvalidDataException("invalid header");

          var modeReader = new SequenceReader<byte>(mode);

          permissions = 0u;

          for (var index = 0; index < mode.Length; index++) {
            modeReader.TryRead(out var octal);

            permissions <<= 3;

            if (!Hexadecimal.TryDecodeValue(octal, out var p) || 0x8 <= p) {
              permissions = 0u;
              break;
              //throw new InvalidDataException("invalid header");
            }

            permissions |= p;
          }

          fileName = OctetEncoding.EightBits.GetString(
#if NET5_0_OR_GREATER
            headerReader.UnreadSequence
#else
            headerReader.Sequence.Slice(headerReader.Position)
#endif
          ).Trim();
#else // #if !(NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER)
        var line = l.Value.Sequence;

        if (line.StartsWith(headerLinePrefix.Span)) {
          const byte SP = 0x20;

          var mode = line.Slice(headerLinePrefix.Span.Length);
          var startOfFile = mode.PositionOf(SP);

          if (!startOfFile.HasValue)
            throw new InvalidDataException("invalid header");

          var file = mode.Slice(startOfFile.Value).Slice(1/*SP*/);

          mode = mode.Slice(0, startOfFile.Value);
          permissions = 0u;

          for (var index = 0; index < mode.Length; index++) {
            permissions <<= 3;

            var octal = mode.Slice(index).First.Span[0];

            if (!Hexadecimal.TryDecodeValue(octal, out var p) || 0x8 <= p) {
              permissions = 0u;
              break;
              //throw new InvalidDataException("invalid header");
            }

            permissions |= p;
          }

          fileName = OctetEncoding.EightBits.GetString(file).Trim();
#endif

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

