// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
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

namespace Smdn {
  public struct FourCC :
    IEquatable<FourCC>,
    IEquatable<string>,
    IEquatable<ByteString>
  {
    public static readonly FourCC Empty = new FourCC(0);

#region "construction"
    public static FourCC CreateBigEndian(int bigEndianInt)
    {
      return new FourCC(bigEndianInt);
    }

    public static FourCC CreateLittleEndian(int littleEndianInt)
    {
      return new FourCC(System.Net.IPAddress.HostToNetworkOrder(littleEndianInt));
    }

    public FourCC(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (@value.Length != 4)
        throw new ArgumentOutOfRangeException("value", "length must be 4");

      this.fourcc = @value[0] << 24 |
                    @value[1] << 16 |
                    @value[2] << 8 |
                    @value[3];
    }

    public FourCC(string value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (@value.Length != 4)
        throw new ArgumentOutOfRangeException("value", "length must be 4");

      this.fourcc = (byte)@value[0] << 24 |
                    (byte)@value[1] << 16 |
                    (byte)@value[2] << 8 |
                    (byte)@value[3];
    }

    public FourCC(ByteString @value)
      : this(@value.ByteArray)
    {
    }

    private FourCC(int fourcc)
    {
      this.fourcc = fourcc;
    }
#endregion

#region "conversion"
    public static implicit operator FourCC(string fourccString)
    {
      return new FourCC(fourccString);
    }

    public static implicit operator string(FourCC fourcc)
    {
      return fourcc.ToString();
    }

    public static implicit operator FourCC(ByteString fourccString)
    {
      return new FourCC(fourccString);
    }

    public static implicit operator ByteString(FourCC fourcc)
    {
      return fourcc.ToByteString();
    }

    public int ToInt32LittleEndian()
    {
      return System.Net.IPAddress.NetworkToHostOrder(fourcc);
    }

    public int ToInt32BigEndian()
    {
      return fourcc;
    }

    public byte[] ToByteArray()
    {
      return new[] {
        (byte)((fourcc >> 24) & 0xff),
        (byte)((fourcc >> 16) & 0xff),
        (byte)((fourcc >>  8) & 0xff),
        (byte)( fourcc        & 0xff),
      };
    }

    public ByteString ToByteString()
    {
      return new ByteString(ToByteArray());
    }

    public override string ToString()
    {
      return new string(new[] {
        (char)((fourcc >> 24) & 0xff),
        (char)((fourcc >> 16) & 0xff),
        (char)((fourcc >>  8) & 0xff),
        (char)( fourcc        & 0xff),
      });
    }

#endregion

#region "comparison"
    public static bool operator == (FourCC x, FourCC y)
    {
      return x.fourcc == y.fourcc;
    }

    public static bool operator != (FourCC x, FourCC y)
    {
      return x.fourcc != y.fourcc;
    }

    public override bool Equals(object obj)
    {
      if (obj is FourCC)
        return Equals((FourCC)obj);
      else if (obj is string)
        return Equals(obj as string);
      else if (obj is ByteString)
        return Equals(obj as ByteString);
      else
        return false;
    }

    public bool Equals(FourCC other)
    {
      return other.fourcc == this.fourcc;
    }

    public bool Equals(string other)
    {
      return string.Equals(this.ToString(), other);
    }

    public bool Equals(ByteString other)
    {
      return this.ToByteString().Equals(other);
    }
#endregion

    public override int GetHashCode()
    {
      return fourcc;
    }

    private int fourcc; // big endian
  }
}
