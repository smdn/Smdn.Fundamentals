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
  public class BinaryWriter : System.IO.BinaryWriter {
    public BinaryWriter(Stream stream)
      : base(stream)
    {
    }

    public void WriteZero(long bytes)
    {
      if (bytes < 0)
        throw new ArgumentOutOfRangeException("bytes", "must be zero or positive number");

      for (; 8L <= bytes; bytes -= 8L)
        Write((UInt64)0);

      for (; 0L < bytes; bytes--)
        Write((byte)0);
    }

    public void WriteBE(short @value)
    {
      WriteBE((ushort)@value);
    }

    public void WriteLE(short @value)
    {
      WriteLE((ushort)@value);
    }

    public void WriteBE(int @value)
    {
      WriteBE((uint)@value);
    }

    public void WriteLE(int @value)
    {
      WriteLE((uint)@value);
    }

    public void WriteBE(long @value)
    {
      WriteBE((ulong)@value);
    }

    public void WriteLE(long @value)
    {
      WriteLE((ulong)@value);
    }

    public void WriteBE(ushort @value)
    {
      Write(new[] {
        (byte)((@value & 0xff00) >> 8),
        (byte) (@value & 0x00ff),
      });
    }

    public void WriteLE(ushort @value)
    {
      Write(new[] {
        (byte) (@value & 0x00ff),
        (byte)((@value & 0xff00) >> 8),
      });
    }

    public void WriteBE(uint @value)
    {
      Write(new[] {
        (byte)((@value & 0xff000000) >> 24),
        (byte)((@value & 0x00ff0000) >> 16),
        (byte)((@value & 0x0000ff00) >> 8),
        (byte) (@value & 0x000000ff),
      });
    }

    public void WriteLE(uint @value)
    {
      Write(new[] {
        (byte) (@value & 0x000000ff),
        (byte)((@value & 0x0000ff00) >> 8),
        (byte)((@value & 0x00ff0000) >> 16),
        (byte)((@value & 0xff000000) >> 24),
      });
    }

    public void WriteBE(ulong @value)
    {
      Write(new[] {
        (byte)((@value & 0xff00000000000000) >> 56),
        (byte)((@value & 0x00ff000000000000) >> 48),
        (byte)((@value & 0x0000ff0000000000) >> 40),
        (byte)((@value & 0x000000ff00000000) >> 32),
        (byte)((@value & 0x00000000ff000000) >> 24),
        (byte)((@value & 0x0000000000ff0000) >> 16),
        (byte)((@value & 0x000000000000ff00) >> 8),
        (byte) (@value & 0x00000000000000ff),
      });
    }

    public void WriteLE(ulong @value)
    {
      Write(new[] {
        (byte) (@value & 0x00000000000000ff),
        (byte)((@value & 0x000000000000ff00) >> 8),
        (byte)((@value & 0x0000000000ff0000) >> 16),
        (byte)((@value & 0x00000000ff000000) >> 24),
        (byte)((@value & 0x000000ff00000000) >> 32),
        (byte)((@value & 0x0000ff0000000000) >> 40),
        (byte)((@value & 0x00ff000000000000) >> 48),
        (byte)((@value & 0xff00000000000000) >> 56),
      });
    }

    public void WriteBE(UInt48 @value)
    {
      Write(@value.ToBigEndianByteArray());
    }

    public void WriteLE(UInt48 @value)
    {
      Write(@value.ToLittleEndianByteArray());
    }
  }
}
