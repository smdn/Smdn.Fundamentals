// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Smdn;

[TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public partial struct UInt24 {
  private const int SizeOfSelf = 3;

  // big endian
  [FieldOffset(0)] public byte Byte0; // 0x 00ff0000
  [FieldOffset(1)] public byte Byte1; // 0x 0000ff00
  [FieldOffset(2)] public byte Byte2; // 0x 000000ff

  public UInt24(ReadOnlySpan<byte> value, bool isBigEndian = false)
  {
    if (value.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(value), SizeOfSelf);

    if (isBigEndian) {
      Byte0 = value[0];
      Byte1 = value[1];
      Byte2 = value[2];
    }
    else {
      Byte0 = value[2];
      Byte1 = value[1];
      Byte2 = value[0];
    }
  }

  private UInt24(uint value)
  {
    unchecked {
      Byte0 = (byte)(value >> 16);
      Byte1 = (byte)(value >> 8);
      Byte2 = (byte)value;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal UInt32 Widen()
    => (UInt32)(Byte0 << 16) |
       (UInt32)(Byte1 << 8) |
       (UInt32)Byte2;

  public override int GetHashCode() => (Byte2 << 16) | (Byte1 << 8) | Byte0;

  /*
   * IConvertible
   */
#pragma warning disable IDE0060
  byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt32());
  sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt32());
  ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)ToUInt32();
  long IConvertible.ToInt64(IFormatProvider provider) => (long)ToInt32();
#pragma warning restore IDE0060

#if FEATURE_GENERIC_MATH
  private static bool TryCreateCore<TOther>(
    TOther value,
    out UInt24 result
  ) where TOther : INumber<TOther>
  {
    result = default;

    int val;

    if (typeof(TOther) == typeof(byte))
      val = (int)((byte)(object)value);
    else if (typeof(TOther) == typeof(sbyte))
      val = (int)((sbyte)(object)value);
    else if (typeof(TOther) == typeof(char))
      val = (int)((char)(object)value);
    else if (typeof(TOther) == typeof(ushort))
      val = (int)((ushort)(object)value);
    else if (typeof(TOther) == typeof(short))
      val = (int)((short)(object)value);
    else if (typeof(TOther) == typeof(uint))
      val = unchecked((int)((uint)(object)value));
    else if (typeof(TOther) == typeof(int))
      val = (int)(object)value;
    else if (typeof(TOther) == typeof(ulong))
      val = unchecked((int)((ulong)(object)value));
    else if (typeof(TOther) == typeof(long))
      val = unchecked((int)((long)(object)value));
    else if (typeof(TOther) == typeof(nuint))
      val = unchecked((int)((nuint)(object)value));
    else if (typeof(TOther) == typeof(nint))
      val = unchecked((int)((nint)(object)value));
    else if (typeof(TOther) == typeof(Half))
      val = (int)((Half)(object)value);
    else if (typeof(TOther) == typeof(float))
      val = unchecked((int)((float)(object)value));
    else if (typeof(TOther) == typeof(double))
      val = unchecked((int)((double)(object)value));
#pragma warning disable IDE0045
    else if (typeof(TOther) == typeof(decimal))
      val = unchecked((int)((decimal)(object)value));
#pragma warning restore IDE0045
    else
      throw UInt24n.CreateTypeIsNotConvertibleException<UInt24, TOther>();

    if (val is < minValue or > maxValue)
      return false; // overflow

    result = new(unchecked((uint)val));

    return true;
  }

  public static UInt24 CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      return new((uint)((byte)(object)value));
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      return val < minValue ? MinValue : new((uint)val);
    }
    else if (typeof(TOther) == typeof(char)) {
      return new((uint)((char)(object)value));
    }
    else if (typeof(TOther) == typeof(ushort)) {
      return new((uint)((ushort)(object)value));
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      return val < 0 ? Zero : new((uint)val);
    }
    else if (typeof(TOther) == typeof(uint)) {
      var val = (uint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val);
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (float)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val);
    }

    throw UInt24n.CreateTypeIsNotConvertibleException<UInt24, TOther>();
  }
#endif
}
