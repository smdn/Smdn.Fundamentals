// 
// Copyright (c) 2019 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
