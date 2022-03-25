// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Smdn;

[TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public partial struct UInt48 {
  private const int SizeOfSelf = 6;

  // big endian
  [FieldOffset(0)] public byte Byte0; // 0x 0000ff00 00000000
  [FieldOffset(1)] public byte Byte1; // 0x 000000ff 00000000
  [FieldOffset(2)] public byte Byte2; // 0x 00000000 ff000000
  [FieldOffset(3)] public byte Byte3; // 0x 00000000 00ff0000
  [FieldOffset(4)] public byte Byte4; // 0x 00000000 0000ff00
  [FieldOffset(5)] public byte Byte5; // 0x 00000000 000000ff

  public UInt48(ReadOnlySpan<byte> value, bool isBigEndian = false)
  {
    if (value.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(value), SizeOfSelf);

    if (isBigEndian) {
      Byte0 = value[0];
      Byte1 = value[1];
      Byte2 = value[2];
      Byte3 = value[3];
      Byte4 = value[4];
      Byte5 = value[5];
    }
    else {
      Byte0 = value[5];
      Byte1 = value[4];
      Byte2 = value[3];
      Byte3 = value[2];
      Byte4 = value[1];
      Byte5 = value[0];
    }
  }

  private UInt48(ulong value)
  {
    unchecked {
      Byte0 = (byte)(value >> 40);
      Byte1 = (byte)(value >> 32);
      Byte2 = (byte)(value >> 24);
      Byte3 = (byte)(value >> 16);
      Byte4 = (byte)(value >> 8);
      Byte5 = (byte)value;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal UInt64 Widen()
    => (UInt64)((UInt64)Byte0 << 40) |
       (UInt64)((UInt64)Byte1 << 32) |
       (UInt64)((UInt64)Byte2 << 24) |
       (UInt64)((UInt64)Byte3 << 16) |
       (UInt64)((UInt64)Byte4 << 8) |
       (UInt64)Byte5;

  public override int GetHashCode()
    => ((Byte3 << 24) | (Byte2 << 16) | (Byte1 << 8) | Byte0) ^ ((Byte5 << 8) | Byte4);

  /*
   * IConvertible
   */
  byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt64());
  sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt64());
  ushort IConvertible.ToUInt16(IFormatProvider provider) => checked((ushort)ToUInt64());
  short IConvertible.ToInt16(IFormatProvider provider) => checked((short)ToInt64());

#if FEATURE_GENERIC_MATH
  private static bool TryCreateCore<TOther>(
    TOther value,
    out UInt48 result
  ) where TOther : INumber<TOther>
  {
    result = default;

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
      throw UInt24n.CreateTypeIsNotConvertibleException<UInt48, TOther>();

    if (val is < minValue or > maxValue)
      return false; // overflow

    result = new(unchecked((ulong)val));

    return true;
  }

  public static UInt48 CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      return new((ulong)((byte)(object)value));
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(char)) {
      return new((ulong)((char)(object)value));
    }
    else if (typeof(TOther) == typeof(ushort)) {
      return new((ulong)((ushort)(object)value));
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(uint)) {
      return new((ulong)((uint)(object)value));
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      return val < minValue ? MinValue : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val);
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (double)((float)(object)value);

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      return val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((ulong)val);
    }

    throw UInt24n.CreateTypeIsNotConvertibleException<UInt48, TOther>();
  }
#endif
}
