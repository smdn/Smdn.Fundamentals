// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Test.NUnit.Constraints.Buffers {
#pragma warning disable CA1716
  public class Is : global::NUnit.Framework.Is {
#pragma warning restore CA1716
    public static ReadOnlyByteMemoryEqualConstraint EqualTo(Memory<byte> expected)
      => new(expected);

    public static ReadOnlyByteMemoryEqualConstraint EqualTo(ReadOnlyMemory<byte> expected)
      => new(expected);

    public static ReadOnlyByteMemoryEqualConstraint EqualTo(byte[] expected)
      => new(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(Memory<T> expected) where T : IEquatable<T>
      => new(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(ReadOnlyMemory<T> expected) where T : IEquatable<T>
      => new(expected);

    public static ReadOnlyMemoryEqualConstraint<T> EqualTo<T>(T[] expected) where T : IEquatable<T>
      => new(expected);
  }
}
