// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework.Constraints;

using Smdn.Text.Unicode.ControlPictures;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class ReadOnlyByteMemoryEqualConstraint : EqualConstraint {
    public ReadOnlyMemory<byte> Expected { get; }
    private Tolerance tolerance = Tolerance.Default;

    public ReadOnlyByteMemoryEqualConstraint(ReadOnlyMemory<byte> expected)
      : base(expected)
    {
      this.Expected = expected;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
      return actual switch {
        ReadOnlyMemory<byte> actualMemory => new ReadOnlyByteMemoryEqualConstraintResult(this, actualMemory, Expected.Span.SequenceEqual(actualMemory.Span)),
        Memory<byte> actualMemory => new ReadOnlyByteMemoryEqualConstraintResult(this, actualMemory, Expected.Span.SequenceEqual(actualMemory.Span)),
        byte[] actualByteArray => new ReadOnlyByteMemoryEqualConstraintResult(this, actualByteArray, Expected.Span.SequenceEqual(actualByteArray.AsSpan())),
        _ => new EqualConstraintResult(this, actual, new NUnitEqualityComparer().AreEqual(Expected, actual, ref tolerance)),
      };
    }

    public override string Description => "ReadOnlyMemory<byte>.SequenceEquals";
  }
}
