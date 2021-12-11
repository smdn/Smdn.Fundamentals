// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;

namespace Smdn.Buffers;

public sealed class DuplicateBufferWriter {
  public static IBufferWriter<T> Create<T>(IBufferWriter<T> firstWriter, IBufferWriter<T> secondWriter)
  {
    if (firstWriter is null)
      throw new ArgumentNullException(nameof(firstWriter));
    if (secondWriter is null)
      throw new ArgumentNullException(nameof(secondWriter));
    if (object.ReferenceEquals(firstWriter, secondWriter))
      throw new ArgumentException("cannot duplicate same IBufferWriter");

    return new Writer<T>(firstWriter, secondWriter);
  }

  private sealed class Writer<T> : IBufferWriter<T> {
    private readonly IBufferWriter<T> firstWriter;
    private readonly IBufferWriter<T> secondWriter;

    private Memory<T> currentMemory;
    private int? currentMemorySize;

    public Writer(IBufferWriter<T> firstWriter, IBufferWriter<T> secondWriter)
    {
      this.firstWriter = firstWriter;
      this.secondWriter = secondWriter;
    }

    public void Advance(int count)
    {
      if (count == 0)
        return; // do nothing
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
      if (currentMemorySize.HasValue && currentMemorySize.Value < count)
        throw new InvalidOperationException("attempted to advance beyond the end of the buffer");
      if (currentMemory.IsEmpty)
        throw new InvalidOperationException("attempted to advance beyond the end of the buffer");

      var memoryFromSecondWriter = secondWriter.GetMemory(count);

      currentMemory.Slice(0, count).CopyTo(memoryFromSecondWriter);

      firstWriter.Advance(count);
      secondWriter.Advance(count);

      currentMemory = currentMemory.Slice(count);

      if (currentMemorySize.HasValue)
        currentMemorySize = currentMemorySize.Value - count;
    }

    public Memory<T> GetMemory(int sizeHint = 0)
    {
      if (sizeHint < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(sizeHint), sizeHint);

      currentMemory = firstWriter.GetMemory(sizeHint);
      currentMemorySize = sizeHint == 0 ? null : sizeHint;

      return currentMemory;
    }

    public Span<T> GetSpan(int sizeHint = 0)
    {
      if (sizeHint < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(sizeHint), sizeHint);

      return GetMemory(sizeHint).Span;
    }
  }
}
