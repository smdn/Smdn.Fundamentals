// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;

namespace Smdn.Text;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class ByteStringExtensions {
  public static ReadOnlySequence<byte> AsReadOnlySequence(this ByteString str)
    => new((str ?? throw new ArgumentNullException(nameof(str))).Segment.AsMemory());

  [Obsolete("ByteString will be deprecated in future.")]
  public static ByteString ToByteString(this ReadOnlySequence<byte> sequence) => ByteString.CreateImmutable(sequence.ToArray());

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
