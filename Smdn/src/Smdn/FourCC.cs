// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
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
using System.Linq;

namespace Smdn {
  /*
   * RFC 2361 - WAVE and AVI Codec Registries [INFORMATIONAL]
   * http://tools.ietf.org/html/rfc2361
   */
  public readonly struct FourCC :
    IEquatable<FourCC>,
    IEquatable<string>,
    IEquatable<byte[]>
  {
    public static readonly FourCC Empty = new FourCC(0);

#region "construction"
    public static FourCC CreateBigEndian(int bigEndianInt)
      => new FourCC(bigEndianInt);

    public static FourCC CreateLittleEndian(int littleEndianInt)
      => new FourCC(System.Net.IPAddress.HostToNetworkOrder(littleEndianInt));

    public FourCC(byte[] @value)
      : this(@value, 0)
    {
    }

    public FourCC(byte[] @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException(nameof(value));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (@value.Length - 4 < startIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), @value, startIndex, 4);

      this.fourcc = @value[startIndex + 0] << 24 |
                    @value[startIndex + 1] << 16 |
                    @value[startIndex + 2] << 8 |
                    @value[startIndex + 3];
    }

    public FourCC(string value)
    {
      if (@value == null)
        throw new ArgumentNullException(nameof(value));
      if (@value.Length != 4)
        throw new ArgumentException("length must be 4", nameof(value));

      checked {
        this.fourcc = (byte)@value[0] << 24 |
                      (byte)@value[1] << 16 |
                      (byte)@value[2] << 8 |
                      (byte)@value[3];
      }
    }

    private FourCC(int fourcc)
    {
      this.fourcc = fourcc;
    }
#endregion

#region "conversion"
    public static implicit operator FourCC(string fourccString)
      => new FourCC(fourccString);

    public static explicit operator string(FourCC fourcc)
      => fourcc.ToString();

    public static explicit operator FourCC(byte[] fourccByteArray)
      => new FourCC(fourccByteArray);

    public static explicit operator byte[](FourCC fourcc)
      => fourcc.ToByteArray();

    public int ToInt32LittleEndian()
      => System.Net.IPAddress.NetworkToHostOrder(fourcc);

    public int ToInt32BigEndian()
      => fourcc;

    /*
     * RFC 2361 - WAVE and AVI Codec Registries
     * 4 Mapping Codec IDs to GUID Values
     *    FourCC values are converted to GUIDs by inserting the FourCC value
     *    into the XXXXXXXX part of the same template: {XXXXXXXX-0000-0010-
     *    8000-00AA00389B71}. For example, a conversion of the FourCC value of
     *    "H260" would result in the GUID value of {30363248-0000-0010-8000-
     *    00AA00389B71}.
     */
    public Guid ToCodecGuid()
      => new Guid(ToInt32LittleEndian(), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

    public void GetBytes(byte[] buffer, int startIndex)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof(buffer));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (buffer.Length - 4 < startIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), buffer, startIndex, 4);

      unchecked {
        buffer[startIndex++] = (byte)(fourcc >> 24);
        buffer[startIndex++] = (byte)(fourcc >> 16);
        buffer[startIndex++] = (byte)(fourcc >>  8);
        buffer[startIndex++] = (byte)(fourcc);
      }
    }

    public byte[] ToByteArray()
    {
      var bytes = new byte[4];

      GetBytes(bytes, 0);

      return bytes;
    }

    public override string ToString()
    {
      unchecked {
        return new string(new[] {
          (char)((fourcc >> 24) & 0xff),
          (char)((fourcc >> 16) & 0xff),
          (char)((fourcc >>  8) & 0xff),
          (char)( fourcc        & 0xff),
        });
      }
    }

#endregion

#region "comparison"
    public static bool operator == (FourCC x, FourCC y)
      => x.fourcc == y.fourcc;

    public static bool operator != (FourCC x, FourCC y)
      => x.fourcc != y.fourcc;

    public override bool Equals(object obj)
    {
      if (obj is FourCC)
        return Equals((FourCC)obj);
      else if (obj is string)
        return Equals(obj as string);
      else if (obj is byte[])
        return Equals(obj as byte[]);
      else
        return false;
    }

    public bool Equals(FourCC other)
      => other.fourcc == this.fourcc;

    public bool Equals(string other)
      => string.Equals(this.ToString(), other);

    public bool Equals(byte[] other)
      => (other != null) && other.SequenceEqual(this.ToByteArray());
#endregion

    public override int GetHashCode()
      => fourcc;

    private readonly int fourcc; // big endian
  }
}
