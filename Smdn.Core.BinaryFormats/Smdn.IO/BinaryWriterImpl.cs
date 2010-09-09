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

namespace Smdn.IO {
  internal static class BinaryWriterImpl {
    private static readonly byte[] zeroBytes = new byte[] {0, 0, 0, 0, 0, 0, 0, 0};

    internal static void WriteZero(System.IO.BinaryWriter w, long bytes)
    {
      if (bytes < 0)
        throw new ArgumentOutOfRangeException("bytes", "must be zero or positive number");

      for (; 8L <= bytes; bytes -= 8L)
        w.Write(zeroBytes);

      for (; 0L < bytes; bytes--)
        w.Write((byte)0);
    }

    internal static void WriteBE(System.IO.BinaryWriter w, short @value)
    {
      WriteBE(w, (ushort)@value);
    }

    internal static void WriteLE(System.IO.BinaryWriter w, short @value)
    {
      WriteLE(w, (ushort)@value);
    }

    internal static void WriteBE(System.IO.BinaryWriter w, int @value)
    {
      WriteBE(w, (uint)@value);
    }

    internal static void WriteLE(System.IO.BinaryWriter w, int @value)
    {
      WriteLE(w, (uint)@value);
    }

    internal static void WriteBE(System.IO.BinaryWriter w, long @value)
    {
      WriteBE(w, (ulong)@value);
    }

    internal static void WriteLE(System.IO.BinaryWriter w, long @value)
    {
      WriteLE(w, (ulong)@value);
    }

    internal static void WriteBE(System.IO.BinaryWriter w, ushort @value)
    {
      w.Write(new[] {
        (byte)((@value & 0xff00) >> 8),
        (byte) (@value & 0x00ff),
      });
    }

    internal static void WriteLE(System.IO.BinaryWriter w, ushort @value)
    {
      w.Write(new[] {
        (byte) (@value & 0x00ff),
        (byte)((@value & 0xff00) >> 8),
      });
    }

    internal static void WriteBE(System.IO.BinaryWriter w, uint @value)
    {
      w.Write(new[] {
        (byte)((@value & 0xff000000) >> 24),
        (byte)((@value & 0x00ff0000) >> 16),
        (byte)((@value & 0x0000ff00) >> 8),
        (byte) (@value & 0x000000ff),
      });
    }

    internal static void WriteLE(System.IO.BinaryWriter w, uint @value)
    {
      w.Write(new[] {
        (byte) (@value & 0x000000ff),
        (byte)((@value & 0x0000ff00) >> 8),
        (byte)((@value & 0x00ff0000) >> 16),
        (byte)((@value & 0xff000000) >> 24),
      });
    }

    internal static void WriteBE(System.IO.BinaryWriter w, ulong @value)
    {
      w.Write(new[] {
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

    internal static void WriteLE(System.IO.BinaryWriter w, ulong @value)
    {
      w.Write(new[] {
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

    internal static void WriteBE(System.IO.BinaryWriter w, UInt24 @value)
    {
      w.Write(@value.ToBigEndianByteArray());
    }

    internal static void WriteLE(System.IO.BinaryWriter w, UInt24 @value)
    {
      w.Write(@value.ToLittleEndianByteArray());
    }

    internal static void WriteBE(System.IO.BinaryWriter w, UInt48 @value)
    {
      w.Write(@value.ToBigEndianByteArray());
    }

    internal static void WriteLE(System.IO.BinaryWriter w, UInt48 @value)
    {
      w.Write(@value.ToLittleEndianByteArray());
    }

    internal static void Write(System.IO.BinaryWriter w, FourCC @value)
    {
      w.Write(@value.ToByteArray());
    }
  }
}
