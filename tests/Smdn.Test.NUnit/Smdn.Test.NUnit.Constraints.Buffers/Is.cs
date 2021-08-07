// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Test.NUnit.Constraints.Buffers {
  public class Is : global::NUnit.Framework.Is {
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(Memory<byte> expected)
      => new ReadOnlyByteMemoryEqualConstraint(expected);

    public static ReadOnlyByteMemoryEqualConstraint EqualTo(ReadOnlyMemory<byte> expected)
      => new ReadOnlyByteMemoryEqualConstraint(expected);

    public static ReadOnlyByteMemoryEqualConstraint EqualTo(byte[] expected)
      => new ReadOnlyByteMemoryEqualConstraint(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(Memory<T> expected) where T : IEquatable<T>
      => new ReadOnlyMemoryEqualConstraint<T>(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(ReadOnlyMemory<T> expected) where T : IEquatable<T>
      => new ReadOnlyMemoryEqualConstraint<T>(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(T[] expected) where T : IEquatable<T>
      => new ReadOnlyMemoryEqualConstraint<T>(expected);
  }
}