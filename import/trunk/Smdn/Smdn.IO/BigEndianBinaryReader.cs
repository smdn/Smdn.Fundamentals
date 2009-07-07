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
  public class BigEndianBinaryReader : System.IO.BinaryReader {
    public BigEndianBinaryReader(Stream stream) : base(stream)
    {
      this.reader = new BinaryReader(stream);
    }

    public override short ReadInt16()
    {
      return reader.ReadInt16BE();
    }

    public override ushort ReadUInt16()
    {
      return reader.ReadUInt16BE();
    }

    public override int ReadInt32()
    {
      return reader.ReadInt32BE();
    }

    public override uint ReadUInt32()
    {
      return reader.ReadUInt32BE();
    }

    public override long ReadInt64()
    {
      return reader.ReadInt64BE();
    }

    public override ulong ReadUInt64()
    {
      return reader.ReadUInt64BE();
    }

    public virtual UInt48 ReadUInt48()
    {
      return reader.ReadUInt48BE();
    }

    private BinaryReader reader;
  }
}
