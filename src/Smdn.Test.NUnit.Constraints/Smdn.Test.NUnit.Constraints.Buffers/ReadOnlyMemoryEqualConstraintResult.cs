// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Linq;

using NUnit.Framework.Constraints;

namespace Smdn.Test.NUnit.Constraints.Buffers;

#pragma warning disable IDE0055
public class ReadOnlyMemoryEqualConstraintResult<T> :
  EqualConstraintResult
  where T : IEquatable<T>
{
#pragma warning restore IDE0055
  private readonly ReadOnlyMemory<T> expectedValue;

  public ReadOnlyMemoryEqualConstraintResult(ReadOnlyMemoryEqualConstraint<T> constraint, object actual, bool hasSucceeded)
    : base(
      constraint ?? throw new ArgumentNullException(nameof(constraint)),
      actual,
      hasSucceeded
    )
  {
    this.expectedValue = constraint.Expected;
  }

  public override void WriteMessageTo(MessageWriter writer)
  {
    if (writer is null)
      throw new ArgumentNullException(nameof(writer));

    writer.WriteMessageLine($"Expected: {{{ToJoinedString(expectedValue)}}} (Length={expectedValue.Length})");

    if (ActualValue is Memory<T> actualMemory)
      writer.WriteMessageLine($"But was: {{{ToJoinedString(actualMemory)}}} (Length={actualMemory.Length})");
    else if (ActualValue is ReadOnlyMemory<T> actualReadOnlyMemory)
      writer.WriteMessageLine($"But was: {{{ToJoinedString(actualReadOnlyMemory)}}} (Length={actualReadOnlyMemory.Length})");
    else if (ActualValue is T[] actualArray)
      writer.WriteMessageLine($"But was: {{{string.Join(", ", actualArray)}}} (Length={actualArray.Length})");
    else
      writer.WriteActualValue(ActualValue);
  }

  private static string ToJoinedString(ReadOnlyMemory<T> memory)
  {
    T[] buffer = null;

    try {
      buffer = ArrayPool<T>.Shared.Rent(memory.Length);

      memory.CopyTo(buffer);

      return string.Join(", ", buffer.Take(memory.Length));
    }
    finally {
      if (buffer is not null)
        ArrayPool<T>.Shared.Return(buffer);
    }
  }
}
