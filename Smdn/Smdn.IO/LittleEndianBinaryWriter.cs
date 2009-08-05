// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
  public class LittleEndianBinaryWriter : System.IO.BinaryWriter {
    public LittleEndianBinaryWriter(Stream stream)
      : base(stream)
    {
    }

    public void WriteZero(long bytes)
    {
      BinaryWriterImpl.WriteZero(this, bytes);
    }

    public override void Write(short @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public override void Write(ushort @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public override void Write(int @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public override void Write(uint @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public override void Write(long @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public override void Write(ulong @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public virtual void Write(UInt24 @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public virtual void Write(UInt48 @value)
    {
      BinaryWriterImpl.WriteLE(this, @value);
    }

    public virtual void Write(FourCC @value)
    {
      BinaryWriterImpl.Write(this, @value);
    }
  }
}
