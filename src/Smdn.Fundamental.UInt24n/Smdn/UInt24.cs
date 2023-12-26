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

  private UInt24(uint value, bool check)
  {
    if (check && maxValue < value)
      throw UInt24n.CreateOverflowException<UInt24>(value);

    unchecked {
      Byte0 = (byte)(value >> 16);
      Byte1 = (byte)(value >> 8);
      Byte2 = (byte)value;
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal readonly UInt32 Widen()
    => (UInt32)(Byte0 << 16) |
       (UInt32)(Byte1 << 8) |
       (UInt32)Byte2;

  public override readonly int GetHashCode() => (Byte2 << 16) | (Byte1 << 8) | Byte0;

  /*
   * IConvertible
   */
#pragma warning disable IDE0060
  readonly byte IConvertible.ToByte(IFormatProvider provider) => checked((byte)ToUInt32());
  readonly sbyte IConvertible.ToSByte(IFormatProvider provider) => checked((sbyte)ToInt32());
  readonly ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong)ToUInt32();
  readonly long IConvertible.ToInt64(IFormatProvider provider) => (long)ToInt32();
#pragma warning restore IDE0060

  /*
   * IBinaryInteger<TSelf>
   */
  public readonly bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
  {
    bytesWritten = default;

    if (destination.Length < SizeOfSelf)
      return false;

    destination[bytesWritten++] = Byte0;
    destination[bytesWritten++] = Byte1;
    destination[bytesWritten++] = Byte2;

    return true;
  }

  public readonly bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
  {
    bytesWritten = default;

    if (destination.Length < SizeOfSelf)
      return false;

    destination[bytesWritten++] = Byte2;
    destination[bytesWritten++] = Byte1;
    destination[bytesWritten++] = Byte0;

    return true;
  }

#if GENERIC_MATH_INTERFACES
  /*
   * INumberBase<TSelf>
   */
  private static bool TryConvertFromTruncating<TOther>(
    TOther value,
    bool throwIfOverflow,
    out UInt24 result,
    out bool overflow
  ) where TOther : INumberBase<TOther>
  {
    result = default;
    overflow = default;

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
      return false; // is not convertible

    overflow = val is < minValue or > maxValue;

    if (overflow && throwIfOverflow)
      throw UInt24n.CreateOverflowException<UInt24>(value);

    result = new(unchecked((uint)val), check: false);

    return true;
  }

#pragma warning disable CA1502 // TODO: refactor
  /*
   * INumberBase<TOther>.TryConvertFromSaturating
   */
  public static bool TryConvertFromSaturating<TOther>(TOther value, out UInt24 result) where TOther : INumberBase<TOther>
  {
    if (typeof(TOther) == typeof(byte)) {
      result = new((uint)((byte)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(sbyte)) {
      var val = (sbyte)(object)value;

      result = val < minValue ? MinValue : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(char)) {
      result = new((uint)((char)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(ushort)) {
      result = new((uint)((ushort)(object)value), check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(short)) {
      var val = (short)(object)value;

      result = val < 0 ? Zero : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(uint)) {
      var val = (uint)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new(val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(int)) {
      var val = (int)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(ulong)) {
      var val = (ulong)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(long)) {
      var val = (long)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(nuint)) {
      var val = (nuint)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(nint)) {
      var val = (nint)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(Half)) {
      var val = (float)((Half)(object)value);

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(float)) {
      var val = (float)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(double)) {
      var val = (double)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }
    else if (typeof(TOther) == typeof(decimal)) {
      var val = (decimal)(object)value;

      result = val < minValue ? MinValue
        : val > maxValue ? MaxValue
        : new((uint)val, check: false);

      return true;
    }

    result = default;

    return false;
  }
#pragma warning restore CA1502

#endif
}
