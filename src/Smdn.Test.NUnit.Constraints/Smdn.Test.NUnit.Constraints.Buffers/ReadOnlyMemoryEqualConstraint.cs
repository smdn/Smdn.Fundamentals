// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Linq;

using NUnit.Framework.Constraints;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class ReadOnlyMemoryEqualConstraintResult<T> :
    EqualConstraintResult
    where T : IEquatable<T>
  {
    private readonly ReadOnlyMemory<T> expectedValue;

    public ReadOnlyMemoryEqualConstraintResult(ReadOnlyMemoryEqualConstraint<T> constraint, object actual, bool hasSucceeded)
      : base(constraint, actual, hasSucceeded)
    {
      this.expectedValue = constraint.Expected;
    }

    public override void WriteMessageTo(MessageWriter writer)
    {
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

  public class ReadOnlyMemoryEqualConstraint<T> :
    EqualConstraint
    where T : IEquatable<T>
  {
    public ReadOnlyMemory<T> Expected { get; }
    private Tolerance _tolerance = Tolerance.Default;

    public ReadOnlyMemoryEqualConstraint(ReadOnlyMemory<T> expected)
      : base(expected)
    {
      this.Expected = expected;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
      return actual switch {
        ReadOnlyMemory<T> actualMemory => new ReadOnlyMemoryEqualConstraintResult<T>(this, actualMemory, Expected.Span.SequenceEqual(actualMemory.Span)),
        T[] actualArray => new ReadOnlyMemoryEqualConstraintResult<T>(this, actualArray, Expected.Span.SequenceEqual(actualArray.AsSpan())),
        _ => new EqualConstraintResult(this, actual, new NUnitEqualityComparer().AreEqual(Expected, actual, ref _tolerance))
      };
    }

    public override string Description => "ReadOnlyMemory<T>.SequenceEquals";
  }
}