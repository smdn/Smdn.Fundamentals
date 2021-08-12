// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn.Text {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ByteStringExtensions {
    public static ReadOnlySequence<byte> AsReadOnlySequence(this ByteString str)
    {
      return new ReadOnlySequence<byte>(str.Segment.AsMemory());
    }

    [Obsolete()]
    public static ByteString ToByteString(this ReadOnlySequence<byte> sequence)
    {
      return ByteString.CreateImmutable(sequence.ToArray());
    }

    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqual instead")]
    public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
      => Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqual(sequence, value);

    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.StartsWith instead")]
    public static bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> value)
      => Smdn.Buffers.ReadOnlySequenceExtensions.StartsWith(sequence, value);

    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqualIgnoreCase instead", error: true)]
    public static unsafe byte[] ToArrayUpperCase(this ReadOnlySequence<byte> sequence)
      => throw new NotSupportedException("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqualIgnoreCase instead");
  }
}
