// Smdn.Test.NUnit.Constraints.dll (Smdn.Test.NUnit.Constraints-1.0.0)
//   Name: Smdn.Test.NUnit.Constraints
//   AssemblyVersion: 1.0.0.0
//   InformationalVersion: 1.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
#nullable enable annotations

using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Smdn.Test.NUnit.Constraints.Buffers;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class Is : Is {
    public Is() {}

    public static ReadOnlyByteMemoryEqualConstraint EqualTo(Memory<byte> expected) {}
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(ReadOnlyMemory<byte> expected) {}
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(byte[] expected) {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(Memory<T> expected) where T : IEquatable<T> {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(ReadOnlyMemory<T> expected) where T : IEquatable<T> {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(T[] expected) where T : IEquatable<T> {}
  }

  public class ReadOnlyByteMemoryEqualConstraint : EqualConstraint {
    public ReadOnlyByteMemoryEqualConstraint(ReadOnlyMemory<byte> expected) {}

    public override string Description { get; }
    public ReadOnlyMemory<byte> Expected { get; }

    public override ConstraintResult ApplyTo<TActual>(TActual actual) {}
  }

  public class ReadOnlyByteMemoryEqualConstraintResult : EqualConstraintResult {
    public ReadOnlyByteMemoryEqualConstraintResult(ReadOnlyByteMemoryEqualConstraint constraint, object actual, bool hasSucceeded) {}

    public override void WriteMessageTo(MessageWriter writer) {}
  }

  public class ReadOnlyMemoryEqualConstraintResult<T> : EqualConstraintResult where T : IEquatable<T> {
    public ReadOnlyMemoryEqualConstraintResult(ReadOnlyMemoryEqualConstraint<T> constraint, object actual, bool hasSucceeded) {}

    public override void WriteMessageTo(MessageWriter writer) {}
  }

  public class ReadOnlyMemoryEqualConstraint<T> : EqualConstraint where T : IEquatable<T> {
    public ReadOnlyMemoryEqualConstraint(ReadOnlyMemory<T> expected) {}

    public override string Description { get; }
    public ReadOnlyMemory<T> Expected { get; }

    public override ConstraintResult ApplyTo<TActual>(TActual actual) {}
  }
}
