// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Linq;

using NUnit.Framework.Constraints;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class ReadOnlyMemoryEqualConstraint<T> :
    EqualConstraint
    where T : IEquatable<T>
  {
    public ReadOnlyMemory<T> Expected { get; }
    private Tolerance tolerance = Tolerance.Default;

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
        _ => new EqualConstraintResult(this, actual, new NUnitEqualityComparer().AreEqual(Expected, actual, ref tolerance)),
      };
    }

    public override string Description => "ReadOnlyMemory<T>.SequenceEquals";
  }
}
