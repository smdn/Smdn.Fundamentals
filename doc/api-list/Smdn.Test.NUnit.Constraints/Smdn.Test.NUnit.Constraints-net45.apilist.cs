// Smdn.Test.NUnit.Constraints.dll (Smdn.Test.NUnit.Constraints-1.0.1)
//   Name: Smdn.Test.NUnit.Constraints
//   AssemblyVersion: 1.0.1.0
//   InformationalVersion: 1.0.1+22d13e8bac8188bdb5b18a03d0357887e371a50f
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.ControlPicture, Version=3.0.0.1, Culture=neutral
//     System.Buffers, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
//     nunit.framework, Version=3.13.0.0, Culture=neutral, PublicKeyToken=2638cd05610744eb

using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Smdn.Test.NUnit.Constraints.Buffers;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class Is : Is {
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(Memory<byte> expected) {}
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(ReadOnlyMemory<byte> expected) {}
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(byte[] expected) {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(Memory<T> expected) where T : IEquatable<T> {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(ReadOnlyMemory<T> expected) where T : IEquatable<T> {}
    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(T[] expected) where T : IEquatable<T> {}

    public Is() {}
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
