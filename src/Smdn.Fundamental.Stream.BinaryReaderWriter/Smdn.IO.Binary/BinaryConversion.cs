// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.IO.Binary {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class BinaryConversion {
    private static void CheckSourceArray(byte[] @value, int startIndex, int count)
    {
      if (@value == null)
        throw new ArgumentNullException(nameof(value));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (@value.Length - count < startIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), @value, startIndex, count);
    }

    private static void CheckDestArray(byte[] @bytes, int startIndex, int count)
    {
      if (@bytes == null)
        throw new ArgumentNullException(nameof(bytes));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (@bytes.Length - count < startIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), @bytes, startIndex, count);
    }

    public static Int16 ByteSwap(Int16 @value)
    {
      unchecked {
        return (Int16)(((@value >> 8) & 0x00ff) | (@value << 8));
      }
    }

    [CLSCompliant(false)]
    public static UInt16 ByteSwap(UInt16 @value)
    {
      unchecked {
        return (UInt16)(((@value >> 8) & 0x00ff) | (@value << 8));
      }
    }

    public static Int32 ByteSwap(Int32 @value)
    {
      unchecked {
        return (Int32)(((@value >> 24) & 0x000000ff) |
                       ((@value >>  8) & 0x0000ff00) |
                       ((@value <<  8) & 0x00ff0000) |
                        (@value << 24));
      }
    }

    [CLSCompliant(false)]
    public static UInt32 ByteSwap(UInt32 @value)
    {
      unchecked {
        return (UInt32)(((@value >> 24) & 0x000000ff) |
                        ((@value >>  8) & 0x0000ff00) |
                        ((@value <<  8) & 0x00ff0000) |
                         (@value << 24));
      }
    }

    public static Int64 ByteSwap(Int64 @value)
    {
      unchecked {
        return (Int64)(((@value >> 56) & 0x00000000000000ff) |
                       ((@value >> 40) & 0x000000000000ff00) |
                       ((@value >> 24) & 0x0000000000ff0000) |
                       ((@value >>  8) & 0x00000000ff000000) |
                       ((@value <<  8) & 0x000000ff00000000) |
                       ((@value << 24) & 0x0000ff0000000000) |
                       ((@value << 40) & 0x00ff000000000000) |
                        (@value << 56));
      }
    }

    [CLSCompliant(false)]
    public static UInt64 ByteSwap(UInt64 @value)
    {
      unchecked {
        return (UInt64)(((@value >> 56) & 0x00000000000000ff) |
                        ((@value >> 40) & 0x000000000000ff00) |
                        ((@value >> 24) & 0x0000000000ff0000) |
                        ((@value >>  8) & 0x00000000ff000000) |
                        ((@value <<  8) & 0x000000ff00000000) |
                        ((@value << 24) & 0x0000ff0000000000) |
                        ((@value << 40) & 0x00ff000000000000) |
                         (@value << 56));
      }
    }

    public static Int16 ToInt16LE(byte[] @value, int startIndex)
    {
      return unchecked((Int16)ToUInt16LE(@value, startIndex));
    }

    public static Int16 ToInt16BE(byte[] @value, int startIndex)
    {
      return unchecked((Int16)ToUInt16BE(@value, startIndex));
    }

    public static Int16 ToInt16(byte[] @value, int startIndex, bool asLittleEndian)
    {
      return unchecked((Int16)ToUInt16(@value, startIndex, asLittleEndian));
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 2);

      return (UInt16)(@value[startIndex] |
                      @value[startIndex + 1] << 8);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 2);

      return (UInt16)(@value[startIndex] << 8 |
                      @value[startIndex + 1]);
    }

    [CLSCompliant(false)]
    public static UInt16 ToUInt16(byte[] @value, int startIndex, bool asLittleEndian)
      => asLittleEndian
        ? ToUInt16LE(@value, startIndex)
        : ToUInt16BE(@value, startIndex);

    public static Int32 ToInt32LE(byte[] @value, int startIndex)
    {
      return unchecked((Int32)ToUInt32LE(@value, startIndex));
    }

    public static Int32 ToInt32BE(byte[] @value, int startIndex)
    {
      return unchecked((Int32)ToUInt32BE(@value, startIndex));
    }

    public static Int32 ToInt32(byte[] @value, int startIndex, bool asLittleEndian)
    {
      return unchecked((Int32)ToUInt32(@value, startIndex, asLittleEndian));
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 4);

      return (UInt32)(@value[startIndex] |
                      @value[startIndex + 1] << 8 |
                      @value[startIndex + 2] << 16 |
                      @value[startIndex + 3] << 24);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 4);

      return (UInt32)(@value[startIndex] << 24 |
                      @value[startIndex + 1] << 16 |
                      @value[startIndex + 2] << 8 |
                      @value[startIndex + 3]);
    }

    [CLSCompliant(false)]
    public static UInt32 ToUInt32(byte[] @value, int startIndex, bool asLittleEndian)
      => asLittleEndian
        ? ToUInt32LE(@value, startIndex)
        : ToUInt32BE(@value, startIndex);

    public static Int64 ToInt64LE(byte[] @value, int startIndex)
    {
      return unchecked((Int64)ToUInt64LE(@value, startIndex));
    }

    public static Int64 ToInt64BE(byte[] @value, int startIndex)
    {
      return unchecked((Int64)ToUInt64BE(@value, startIndex));
    }

    public static Int64 ToInt64(byte[] @value, int startIndex, bool asLittleEndian)
    {
      return unchecked((Int64)ToUInt64(@value, startIndex, asLittleEndian));
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 8);

      UInt64 low  = (UInt32)(@value[startIndex] |
                             @value[startIndex + 1] << 8 |
                             @value[startIndex + 2] << 16 |
                             @value[startIndex + 3] << 24);
      UInt64 high = (UInt32)(@value[startIndex + 4 ] |
                             @value[startIndex + 5] << 8 |
                             @value[startIndex + 6] << 16 |
                             @value[startIndex + 7] << 24);

      return high << 32 | low;
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 8);

      UInt64 high = (UInt32)(@value[startIndex] << 24 |
                             @value[startIndex + 1] << 16 |
                             @value[startIndex + 2] << 8 |
                             @value[startIndex + 3]);
      UInt64 low  = (UInt32)(@value[startIndex + 4] << 24 |
                             @value[startIndex + 5] << 16 |
                             @value[startIndex + 6] << 8 |
                             @value[startIndex + 7]);

      return high << 32 | low;
    }

    [CLSCompliant(false)]
    public static UInt64 ToUInt64(byte[] @value, int startIndex, bool asLittleEndian)
      => asLittleEndian
        ? ToUInt64LE(@value, startIndex)
        : ToUInt64BE(@value, startIndex);

    public static UInt24 ToUInt24LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 3);

      return new UInt24(@value, startIndex, false);
    }

    public static UInt24 ToUInt24BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 3);

      return new UInt24(@value, startIndex, true);
    }

    public static UInt24 ToUInt24(byte[] @value, int startIndex, bool asLittleEndian)
      => asLittleEndian
        ? ToUInt24LE(@value, startIndex)
        : ToUInt24BE(@value, startIndex);

    public static UInt48 ToUInt48LE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 6);

      return new UInt48(@value, startIndex, false);
    }

    public static UInt48 ToUInt48BE(byte[] @value, int startIndex)
    {
      CheckSourceArray(@value, startIndex, 6);

      return new UInt48(@value, startIndex, true);
    }

    public static UInt48 ToUInt48(byte[] @value, int startIndex, bool asLittleEndian)
      => asLittleEndian
        ? ToUInt48LE(@value, startIndex)
        : ToUInt48BE(@value, startIndex);

    public static void GetBytesLE(Int16 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt16)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int16 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt16)@value), bytes, startIndex);
    }

    public static void GetBytes(Int16 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt16)@value), asLittleEndian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt16 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 2);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt16 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 2);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 8);
        bytes[startIndex + 1] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt16 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      if (asLittleEndian)
        GetBytesLE(@value, bytes, startIndex);
      else
        GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytesLE(Int32 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt32)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int32 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt32)@value), bytes, startIndex);
    }

    public static void GetBytes(Int32 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt32)@value), asLittleEndian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt32 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 4);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
        bytes[startIndex + 2] = (byte)(@value >> 16);
        bytes[startIndex + 3] = (byte)(@value >> 24);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt32 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 4);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 24);
        bytes[startIndex + 1] = (byte)(@value >> 16);
        bytes[startIndex + 2] = (byte)(@value >> 8);
        bytes[startIndex + 3] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt32 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      if (asLittleEndian)
        GetBytesLE(@value, bytes, startIndex);
      else
        GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytesLE(Int64 @value, byte[] bytes, int startIndex)
    {
      GetBytesLE(unchecked((UInt64)@value), bytes, startIndex);
    }

    public static void GetBytesBE(Int64 @value, byte[] bytes, int startIndex)
    {
      GetBytesBE(unchecked((UInt64)@value), bytes, startIndex);
    }

    public static void GetBytes(Int64 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      GetBytes(unchecked((UInt64)@value), asLittleEndian, bytes, startIndex);
    }

    [CLSCompliant(false)]
    public static void GetBytesLE(UInt64 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 8);

      unchecked {
        bytes[startIndex    ] = (byte)(@value);
        bytes[startIndex + 1] = (byte)(@value >> 8);
        bytes[startIndex + 2] = (byte)(@value >> 16);
        bytes[startIndex + 3] = (byte)(@value >> 24);
        bytes[startIndex + 4] = (byte)(@value >> 32);
        bytes[startIndex + 5] = (byte)(@value >> 40);
        bytes[startIndex + 6] = (byte)(@value >> 48);
        bytes[startIndex + 7] = (byte)(@value >> 56);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytesBE(UInt64 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 8);

      unchecked {
        bytes[startIndex    ] = (byte)(@value >> 56);
        bytes[startIndex + 1] = (byte)(@value >> 48);
        bytes[startIndex + 2] = (byte)(@value >> 40);
        bytes[startIndex + 3] = (byte)(@value >> 32);
        bytes[startIndex + 4] = (byte)(@value >> 24);
        bytes[startIndex + 5] = (byte)(@value >> 16);
        bytes[startIndex + 6] = (byte)(@value >> 8);
        bytes[startIndex + 7] = (byte)(@value);
      }
    }

    [CLSCompliant(false)]
    public static void GetBytes(UInt64 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      if (asLittleEndian)
        GetBytesLE(@value, bytes, startIndex);
      else
        GetBytesBE(@value, bytes, startIndex);
    }

    public static byte[] GetBytesLE(Int16 @value)
    {
      var bytes = new byte[2];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int16 @value)
    {
      var bytes = new byte[2];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int16 @value, bool asLittleEndian)
    {
      var bytes = new byte[2];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt16 @value)
    {
      var bytes = new byte[2];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt16 @value)
    {
      var bytes = new byte[2];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt16 @value, bool asLittleEndian)
    {
      var bytes = new byte[2];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(Int32 @value)
    {
      var bytes = new byte[4];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int32 @value)
    {
      var bytes = new byte[4];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int32 @value, bool asLittleEndian)
    {
      var bytes = new byte[4];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt32 @value)
    {
      var bytes = new byte[4];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt32 @value)
    {
      var bytes = new byte[4];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt32 @value, bool asLittleEndian)
    {
      var bytes = new byte[4];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(Int64 @value)
    {
      var bytes = new byte[8];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(Int64 @value)
    {
      var bytes = new byte[8];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(Int64 @value, bool asLittleEndian)
    {
      var bytes = new byte[8];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesLE(UInt64 @value)
    {
      var bytes = new byte[8];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytesBE(UInt64 @value)
    {
      var bytes = new byte[8];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    [CLSCompliant(false)]
    public static byte[] GetBytes(UInt64 @value, bool asLittleEndian)
    {
      var bytes = new byte[8];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    public static void GetBytesLE(UInt24 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 3);

      UInt32 val = @value.ToUInt32();

      unchecked {
        bytes[startIndex] = (byte)(val);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val >> 16);
      }
    }

    public static void GetBytesBE(UInt24 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 3);

      UInt32 val = @value.ToUInt32();

      unchecked {
        bytes[startIndex] = (byte)(val >> 16);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val);
      }
    }

    public static void GetBytes(UInt24 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      if (asLittleEndian)
        GetBytesLE(@value, bytes, startIndex);
      else
        GetBytesBE(@value, bytes, startIndex);
    }

    public static void GetBytesLE(UInt48 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 6);

      UInt64 val = @value.ToUInt64();

      unchecked {
        bytes[startIndex] = (byte)(val);
        bytes[startIndex + 1] = (byte)(val >> 8);
        bytes[startIndex + 2] = (byte)(val >> 16);
        bytes[startIndex + 3] = (byte)(val >> 24);
        bytes[startIndex + 4] = (byte)(val >> 32);
        bytes[startIndex + 5] = (byte)(val >> 40);
      }
    }

    public static void GetBytesBE(UInt48 @value, byte[] bytes, int startIndex)
    {
      CheckDestArray(bytes, startIndex, 6);

      UInt64 val = @value.ToUInt64();

      unchecked {
        bytes[startIndex] = (byte)(val >> 40);
        bytes[startIndex + 1] = (byte)(val >> 32);
        bytes[startIndex + 2] = (byte)(val >> 24);
        bytes[startIndex + 3] = (byte)(val >> 16);
        bytes[startIndex + 4] = (byte)(val >> 8);
        bytes[startIndex + 5] = (byte)(val);
      }
    }

    public static void GetBytes(UInt48 @value, bool asLittleEndian, byte[] bytes, int startIndex)
    {
      if (asLittleEndian)
        GetBytesLE(@value, bytes, startIndex);
      else
        GetBytesBE(@value, bytes, startIndex);
    }

    public static byte[] GetBytesLE(UInt24 @value)
    {
      var bytes = new byte[3];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(UInt24 @value)
    {
      var bytes = new byte[3];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(UInt24 @value, bool asLittleEndian)
    {
      var bytes = new byte[3];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesLE(UInt48 @value)
    {
      var bytes = new byte[6];

      GetBytesLE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytesBE(UInt48 @value)
    {
      var bytes = new byte[6];

      GetBytesBE(@value, bytes, 0);

      return bytes;
    }

    public static byte[] GetBytes(UInt48 @value, bool asLittleEndian)
    {
      var bytes = new byte[6];

      GetBytes(@value, asLittleEndian, bytes, 0);

      return bytes;
    }
  }
}

