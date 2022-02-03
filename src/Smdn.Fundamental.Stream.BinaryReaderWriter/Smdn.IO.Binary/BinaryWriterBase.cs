// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#pragma warning disable SA1121 // Use built-in type alias

using System;
using System.IO;

namespace Smdn.IO.Binary;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public abstract class BinaryWriterBase : IDisposable {
  public bool LeaveBaseStreamOpen {
    get { CheckDisposed(); return leaveBaseStreamOpen; }
  }

  public Stream BaseStream {
    get { CheckDisposed(); return stream; }
  }

  protected bool Disposed { get; private set; } = false;

  protected BinaryWriterBase(Stream baseStream, bool leaveBaseStreamOpen)
  {
    if (baseStream == null)
      throw new ArgumentNullException(nameof(baseStream));
    if (!baseStream.CanWrite)
      throw ExceptionUtils.CreateArgumentMustBeWritableStream(nameof(baseStream));

    this.stream = baseStream;
    this.leaveBaseStreamOpen = leaveBaseStreamOpen;
  }

  public virtual void Close() => (this as IDisposable).Dispose();

  void IDisposable.Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing && stream != null) {
      if (!LeaveBaseStreamOpen)
        stream.Close();

      stream = null;
    }

    Disposed = true;
  }

  public void Flush()
  {
    CheckDisposed();

    stream.Flush();
  }

  public virtual void Write(Byte @value) => stream.WriteByte(@value);

  [CLSCompliant(false)]
  public virtual void Write(SByte @value) => stream.WriteByte(unchecked((byte)@value));

  public abstract void Write(Int16 @value);

  [CLSCompliant(false)]
  public virtual void Write(UInt16 @value) => Write(unchecked((Int16)@value));

  public abstract void Write(Int32 @value);

  [CLSCompliant(false)]
  public virtual void Write(UInt32 @value) => Write(unchecked((Int32)@value));

  public abstract void Write(Int64 @value);

  [CLSCompliant(false)]
  public virtual void Write(UInt64 @value) => Write(unchecked((Int64)@value));

  public void Write(byte[] buffer)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));

    Write(buffer, 0, buffer.Length);
  }

  public void Write(byte[] buffer, int index, int count)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (index < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
    if (buffer.Length - count < index)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(index), buffer, index, count);

    if (count == 0)
      return;

    WriteUnchecked(buffer, index, count);
  }

  public void Write(ArraySegment<byte> @value)
  {
    if (@value.Array == null)
      throw new ArgumentException("value.Array is null", nameof(value));

    if (@value.Count == 0)
      return;

    WriteUnchecked(@value.Array, @value.Offset, @value.Count);
  }

  protected void WriteUnchecked(byte[] buffer, int index, int count)
  {
    CheckDisposed();

    stream.Write(buffer, index, count);
  }

  public void WriteZero(int count)
    => WriteZero((long)count);

  public void WriteZero(long count)
  {
    if (count < 0L)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (count == 0L)
      return;

    CheckDisposed();

    var zeroes = new byte[Math.Min(count, 4096)]; // TODO: array pool

    for (; 0 < count; count -= zeroes.Length) {
      if (zeroes.Length < count)
        stream.Write(zeroes, 0, zeroes.Length);
      else
        stream.Write(zeroes, 0, (int)count);
    }
  }

  protected void CheckDisposed()
  {
    if (Disposed)
      throw new ObjectDisposedException(GetType().FullName);
  }

  private Stream stream;
  private readonly bool leaveBaseStreamOpen;
}
