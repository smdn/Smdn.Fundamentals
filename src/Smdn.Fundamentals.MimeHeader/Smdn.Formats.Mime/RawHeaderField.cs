// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using Smdn.Text;

namespace Smdn.Formats.Mime {
  public readonly struct RawHeaderField {
    public ReadOnlySequence<byte> HeaderFieldSequence { get; }
    public int OffsetOfDelimiter { get; }

    public ReadOnlySequence<byte> Name => HeaderFieldSequence.Slice(0, OffsetOfDelimiter);
    public ReadOnlySequence<byte> Value => HeaderFieldSequence.Slice(OffsetOfDelimiter).Slice(1); // offset + 1
    public string NameString => ByteString.ToString(Name);
    public string ValueString => ByteString.ToString(Value);

    internal RawHeaderField(ReadOnlySequence<byte> headerFieldSequence, int offsetOfDelimiter)
    {
      this.HeaderFieldSequence = headerFieldSequence;
      this.OffsetOfDelimiter = offsetOfDelimiter;
    }
  }
}
