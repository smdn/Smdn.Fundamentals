// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

public static class MathShim {
#if !SYSTEM_MATH_DIVREM
  public static int DivRem(int a, int b, out int result)
  {
    var quotient = a / b;

    result = a - (b * quotient);

    return quotient;
  }

  public static long DivRem(long a, long b, out long result)
  {
    var quotient = a / b;

    result = a - (b * quotient);

    return quotient;
  }
#endif

#if !SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  [CLSCompliant(false)]
  public static (sbyte Quotient, sbyte Remainder) DivRem(sbyte left, sbyte right)
  {
    var quotient = checked((sbyte)(left / right));

    return (quotient, checked((sbyte)(left - (right * quotient))));
  }

  public static (short Quotient, short Remainder) DivRem(short left, short right)
  {
    var quotient = checked((short)(left / right));

    return (quotient, checked((short)(left - (right * quotient))));
  }

  public static (int Quotient, int Remainder) DivRem(int left, int right)
  {
    var quotient = checked(left / right);

    return (quotient, checked(left - (right * quotient)));
  }

  public static (long Quotient, long Remainder) DivRem(long left, long right)
  {
    var quotient = checked(left / right);

    return (quotient, checked(left - (right * quotient)));
  }

  public static (byte Quotient, byte Remainder) DivRem(byte left, byte right)
  {
    var quotient = checked((byte)(left / right));

    return (quotient, checked((byte)(left - (right * quotient))));
  }

  [CLSCompliant(false)]
  public static (ushort Quotient, ushort Remainder) DivRem(ushort left, ushort right)
  {
    var quotient = checked((ushort)(left / right));

    return (quotient, checked((ushort)(left - (right * quotient))));
  }

  [CLSCompliant(false)]
  public static (uint Quotient, uint Remainder) DivRem(uint left, uint right)
  {
    var quotient = checked(left / right);

    return (quotient, checked(left - (right * quotient)));
  }

  [CLSCompliant(false)]
  public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
  {
    var quotient = checked(left / right);

    return (quotient, checked(left - (right * quotient)));
  }
#endif
}
