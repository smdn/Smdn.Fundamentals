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
}
