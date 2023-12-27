// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if GENERIC_MATH_INTERFACES
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Smdn;

[TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly partial struct UInt48 {
  private const int SizeOfSelf = 6;

  // big endian
  [FieldOffset(0)] private readonly byte byte0; // 0x 0000ff00 00000000
  [FieldOffset(1)] private readonly byte byte1; // 0x 000000ff 00000000
  [FieldOffset(2)] private readonly byte byte2; // 0x 00000000 ff000000
  [FieldOffset(3)] private readonly byte byte3; // 0x 00000000 00ff0000
  [FieldOffset(4)] private readonly byte byte4; // 0x 00000000 0000ff00
  [FieldOffset(5)] private readonly byte byte5; // 0x 00000000 000000ff

  public UInt48(ReadOnlySpan<byte> value, bool isBigEndian = false)
  {
    if (value.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(value), SizeOfSelf);

    if (isBigEndian) {
      byte0 = value[0];
      byte1 = value[1];
      byte2 = value[2];
      byte3 = value[3];
      byte4 = value[4];
      byte5 = value[5];
    }
    else {
      byte0 = value[5];
      byte1 = value[4];
      byte2 = value[3];
      byte3 = value[2];
      byte4 = value[1];
      byte5 = value[0];
    }
  }

  private UInt48(ulong value, bool check)
  {
    if (check && maxValue < value)
      throw UInt24n.CreateOverflowException<UInt48>(value);

    unchecked {
      byte0 = (byte)(value >> 40);
      byte1 = (byte)(value >> 32);
      byte2 = (byte)(value >> 24);
      byte3 = (byte)(value >> 16);
      byte4 = (byte)(value >> 8);
      byte5 = (byte)value;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal UInt64 Widen()
    => (UInt64)((UInt64)byte0 << 40) |
       (UInt64)((UInt64)byte1 << 32) |
       (UInt64)((UInt64)byte2 << 24) |
       (UInt64)((UInt64)byte3 << 16) |
       (UInt64)((UInt64)byte4 << 8) |
       (UInt64)byte5;

  public override int GetHashCode()
    => ((byte3 << 24) | (byte2 << 16) | (byte1 << 8) | byte0) ^ ((byte5 << 8) | byte4);

  /*
   * IConvertible
   */
#pragma warning disable IDE0060
  byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt64());
  sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt64());
  ushort IConvertible.ToUInt16(IFormatProvider provider) => checked((ushort)ToUInt64());
  short IConvertible.ToInt16(IFormatProvider provider) => checked((short)ToInt64());
#pragma warning restore IDE0060

  /*
   * IBinaryInteger<TSelf>
   */
  public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
  {
    bytesWritten = default;

    if (destination.Length < SizeOfSelf)
      return false;

    destination[bytesWritten++] = byte0;
    destination[bytesWritten++] = byte1;
    destination[bytesWritten++] = byte2;
    destination[bytesWritten++] = byte3;
    destination[bytesWritten++] = byte4;
    destination[bytesWritten++] = byte5;

    return true;
  }

  public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
  {
    bytesWritten = default;

    if (destination.Length < SizeOfSelf)
      return false;

    destination[bytesWritten++] = byte5;
    destination[bytesWritten++] = byte4;
    destination[bytesWritten++] = byte3;
    destination[bytesWritten++] = byte2;
    destination[bytesWritten++] = byte1;
    destination[bytesWritten++] = byte0;

    return true;
  }

#if GENERIC_MATH_INTERFACES
  /*
   * INumberBase<TSelf>
   */
  private static bool TryConvertFromTruncating<TOther>(
    TOther value,
    bool throwIfOverflow,
    out UInt48 result,
    out bool overflow
  ) where TOther : INumberBase<TOther>
  {
    result = default;
    overflow = default;

    long val;

    if (typeof(TOther) == typeof(byte))
      val = (long)((byte)(object)value);
    else if (typeof(TOther) == typeof(sbyte))
      val = (long)((sbyte)(object)value);
    else if (typeof(TOther) == typeof(char))
      val = (long)((char)(object)value);
    else if (typeof(TOther) == typeof(ushort))
      val = (long)((ushort)(object)value);
    else if (typeof(TOther) == typeof(short))
      val = (long)((short)(object)value);
    else if (typeof(TOther) == typeof(uint))
      val = (long)((uint)(object)value);
    else if (typeof(TOther) == typeof(int))
      val = (long)((int)(object)value);
    else if (typeof(TOther) == typeof(ulong))
      val = unchecked((long)((ulong)(object)value));
    else if (typeof(TOther) == typeof(long))
      val = (long)(object)value;
    else if (typeof(TOther) == typeof(nuint))
      val = unchecked((long)((nuint)(object)value));
    else if (typeof(TOther) == typeof(nint))
      val = unchecked((long)((nint)(object)value));
    else if (typeof(TOther) == typeof(Half))
      val = (long)((Half)(object)value);
    else if (typeof(TOther) == typeof(float))
      val = unchecked((long)((float)(object)value));
    else if (typeof(TOther) == typeof(double))
      val = unchecked((long)((double)(object)value));
#pragma warning disable IDE0045
    else if (typeof(TOther) == typeof(decimal))
      val = unchecked((long)((decimal)(object)value));
#pragma warning restore IDE0045
    else
      return false; // is not convertible

    overflow = val is < minValue or > maxValue;

    if (overflow && throwIfOverflow)
      throw UInt24n.CreateOverflowException<UInt48>(value);

    result = new(unchecked((ulong)val), check: false);

    return true;
  }

#pragma warning disable CA1502 // TODO: refactor
  /*
   * INumberBase<TOther>.TryConvertFromSaturating
   */
  public static bool TryConvertFromSaturating<TOther>(TOther value, out UInt48 result) where TOther : INumberBase<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      result = new((ulong)((byte)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      result = val < minValue ? MinValue : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(char)) {
      result = new((ulong)((char)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(ushort)) {
      result = new((ulong)((ushort)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      result = val < minValue ? MinValue : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(uint)) {
      result = new((ulong)((uint)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      result = val < minValue ? MinValue : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (double)((float)(object)value);

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val, check: false);

      return true;
    }

    result = default;

    return false;
  }
#pragma warning restore CA1502

#endif
}
