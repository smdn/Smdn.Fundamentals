// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
using System.IO;

namespace Smdn.IO {
  public class BinaryWriter : System.IO.BinaryWriter {
    public BinaryWriter(Stream stream)
      : base(stream)
    {
    }

    public void WriteZero(long bytes)
    {
      BinaryWriterImpl.WriteZero(this, bytes);
    }

    public void WriteBE(short @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    public void WriteLE(short @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public void WriteBE(int @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    public void WriteLE(int @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public void WriteBE(long @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    public void WriteLE(long @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteBE(ushort @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteLE(ushort @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteBE(uint @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteLE(uint @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteBE(ulong @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    [CLSCompliant(false)]
    public void WriteLE(ulong @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public void WriteBE(UInt24 @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    public void WriteLE(UInt24 @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public void WriteBE(UInt48 @value)
    {
      BinaryWriterImpl.WriteBE(this, @value);
    }

    public void WriteLE(UInt48 @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public void Write(FourCC @value)
    {
      BinaryWriterImpl.Write(this, @value);
    }
  }
}
