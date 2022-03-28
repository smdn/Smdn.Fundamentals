// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

public static class MathShim {
  /*
   * SYSTEM_MATH_CLAMP
   */
  [CLSCompliant(false)]
  public static sbyte Clamp(sbyte value, sbyte min, sbyte max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  public static short Clamp(short value, short min, short max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  public static int Clamp(int value, int min, int max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  public static long Clamp(long value, long min, long max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  public static byte Clamp(byte value, byte min, byte max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  [CLSCompliant(false)]
  public static ushort Clamp(ushort value, ushort min, ushort max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  [CLSCompliant(false)]
  public static uint Clamp(uint value, uint min, uint max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  [CLSCompliant(false)]
  public static ulong Clamp(ulong value, ulong min, ulong max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  public static float Clamp(float value, float min, float max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));
  public static double Clamp(double value, double min, double max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));
  public static decimal Clamp(decimal value, decimal min, decimal max) => min <= max ? (value < min ? min : (value > max ? max : value)) : throw ExceptionUtils.CreateArgumentXMustBeLessThanY(min, nameof(min), max, nameof(max));

  /*
   * SYSTEM_MATH_DIVREM
   */
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

  /*
   * SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
   */
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
}
