// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

using NUnit.Framework.Constraints;

using Smdn.Text.Unicode.ControlPictures;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class ReadOnlyByteMemoryEqualConstraintResult : EqualConstraintResult {
    private readonly ReadOnlyMemory<byte> expectedValue;

    public ReadOnlyByteMemoryEqualConstraintResult(ReadOnlyByteMemoryEqualConstraint constraint, object actual, bool hasSucceeded)
      : base(constraint, actual, hasSucceeded)
    {
      this.expectedValue = constraint.Expected;
    }

    public override void WriteMessageTo(MessageWriter writer)
    {
      writer.WriteMessageLine($"Expected: \"{expectedValue.Span.ToControlCharsPicturizedString()}\" ({BitConverter.ToString(expectedValue.ToArray())})");

      if (ActualValue is ReadOnlyMemory<byte> actualMemory)
        writer.WriteMessageLine($"But was: \"{actualMemory.Span.ToControlCharsPicturizedString()}\" ({BitConverter.ToString(actualMemory.ToArray())})");
      else if (ActualValue is byte[] actualByteArray)
        writer.WriteMessageLine($"But was: \"{((ReadOnlySpan<byte>)actualByteArray).ToControlCharsPicturizedString()}\" ({BitConverter.ToString(actualByteArray)})");
      else
        writer.WriteActualValue(ActualValue);
    }
  }

  public class ReadOnlyByteMemoryEqualConstraint : EqualConstraint {
    public ReadOnlyMemory<byte> Expected { get; }
    private Tolerance _tolerance = Tolerance.Default;

    public ReadOnlyByteMemoryEqualConstraint(ReadOnlyMemory<byte> expected)
      : base(expected)
    {
      this.Expected = expected;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
      return actual switch {
        ReadOnlyMemory<byte> actualMemory => new ReadOnlyByteMemoryEqualConstraintResult(this, actualMemory, Expected.Span.SequenceEqual(actualMemory.Span)),
        byte[] actualByteArray => new ReadOnlyByteMemoryEqualConstraintResult(this, actualByteArray, Expected.Span.SequenceEqual(actualByteArray.AsSpan())),
        _ => new EqualConstraintResult(this, actual, new NUnitEqualityComparer().AreEqual(Expected, actual, ref _tolerance))
      };
    }

    public override string Description => "ReadOnlyMemory<byte>.SequenceEquals";
  }
}