// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

using Smdn.IO;

namespace Smdn.IO.Binary {
  public abstract class BinaryReaderBase : IDisposable {
    public bool LeaveBaseStreamOpen {
      get { CheckDisposed(); return leaveBaseStreamOpen; }
    }

    public virtual bool EndOfStream {
      get
      {
        CheckDisposed();

        if (stream.CanSeek) {
          var eos = (stream.ReadByte() < 0);

          if (!eos)
            stream.Seek(-1L, SeekOrigin.Current);

          return eos;
        }
        else {
          return false;
        }
      }
    }

    public Stream BaseStream {
      get { CheckDisposed(); return stream; }
    }

    protected bool Disposed {
      get { return disposed; }
    }

    protected BinaryReaderBase(Stream baseStream, bool leaveBaseStreamOpen)
    {
      if (baseStream == null)
        throw new ArgumentNullException(nameof(baseStream));
      if (!baseStream.CanRead)
        throw ExceptionUtils.CreateArgumentMustBeReadableStream(nameof(baseStream));

      this.stream = baseStream;
      this.leaveBaseStreamOpen = leaveBaseStreamOpen;
    }

    public virtual void Close()
    {
      Dispose(true);
    }

    void IDisposable.Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && stream != null) {
        if (!LeaveBaseStreamOpen)
          stream.Close();

        stream = null;
      }

      disposed = true;
    }

    public virtual Byte ReadByte()
    {
      CheckDisposed();

      var val = stream.ReadByte();

      if (val == -1)
        throw new EndOfStreamException();
      else
        return unchecked((byte)val);
    }

    [CLSCompliant(false)]
    public virtual SByte ReadSByte()
    {
      return unchecked((sbyte)ReadByte());
    }

    public abstract Int16 ReadInt16();

    [CLSCompliant(false)]
    public virtual UInt16 ReadUInt16()
    {
      return unchecked((UInt16)ReadInt16());
    }

    public abstract Int32 ReadInt32();

    [CLSCompliant(false)]
    public virtual UInt32 ReadUInt32()
    {
      return unchecked((UInt32)ReadInt32());
    }

    public abstract Int64 ReadInt64();

    [CLSCompliant(false)]
    public virtual UInt64 ReadUInt64()
    {
      return unchecked((UInt64)ReadInt64());
    }

    public byte[] ReadBytes(int count)
    {
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (count == 0)
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER
        return Array.Empty<byte>();
#else
        return EmptyByteArray.Instance;
#endif

      var buffer = new byte[count];
      var read = ReadBytes(buffer, 0, count, false);

      if (read < count)
        Array.Resize(ref buffer, read);

      return buffer;
    }

    public byte[] ReadBytes(long count)
    {
      if (int.MaxValue < count)
        throw new NotImplementedException();
      else
        return ReadBytes((int)count);
    }

    public byte[] ReadExactBytes(int count)
    {
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (count == 0)
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER
        return Array.Empty<byte>();
#else
        return EmptyByteArray.Instance;
#endif

      var buffer = new byte[count];

      ReadBytes(buffer, 0, count, true);

      return buffer;
    }

    public byte[] ReadExactBytes(long count)
    {
      if (int.MaxValue < count)
        throw new NotImplementedException();
      else
        return ReadExactBytes((int)count);
    }

    public int ReadBytes(byte[] buffer, int index, int count)
    {
      return ReadBytes(buffer, index, count, false);
    }

    public void ReadExactBytes(byte[] buffer, int index, int count)
    {
      ReadBytes(buffer, index, count, true);
    }

    protected int ReadBytes(byte[] buffer, int index, int count, bool readExactBytes)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof(buffer));
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (index < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
      if (buffer.Length - count < index)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(index), buffer, index, count);

      return ReadBytesUnchecked(buffer, index, count, readExactBytes);
    }

    protected virtual int ReadBytesUnchecked(byte[] buffer, int index, int count, bool readExactBytes)
    {
      CheckDisposed();

      int ret = 0;

      for (;;) {
        if (count == 0)
          return ret;

        var read = stream.Read(buffer, index, count);

        if (read == 0) {
          if (readExactBytes && 0 < count)
            throw new EndOfStreamException();
          else
            return ret;
        }

        index += read;
        count -= read;
        ret   += read;
      }
    }

    public virtual byte[] ReadToEnd()
    {
      CheckDisposed();

      if (stream.CanSeek) {
        var remain = stream.Length - stream.Position;

        if (remain <= 0) {
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NET5_0_OR_GREATER
          return Array.Empty<byte>();
#else
          return EmptyByteArray.Instance;
#endif
        }
        else {
          var bufferSize = (int)Math.Min(4096L, remain);
          var initialCapacity = (int)Math.Min((long)int.MaxValue, remain);

          return stream.ReadToEnd(bufferSize, initialCapacity);
        }
      }
      else {
        return stream.ReadToEnd();
      }
    }

    protected void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Stream stream;
    private readonly bool leaveBaseStreamOpen;
    private bool disposed = false;
  }
}
