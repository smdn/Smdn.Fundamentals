// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_BUFFERS_SEQUENCEREADER
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Smdn.Buffers;

public static class SequenceReaderExtensions {
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ReadOnlySequence<T> GetUnreadSequence<T>(this SequenceReader<T> sequenceReader) where T : unmanaged, IEquatable<T>
#if SYSTEM_BUFFERS_SEQUENCEREADER_UNREADSEQUENCE
    => sequenceReader.UnreadSequence;
#else
    => sequenceReader.Sequence.Slice(sequenceReader.Position);
#endif
}
#endif
