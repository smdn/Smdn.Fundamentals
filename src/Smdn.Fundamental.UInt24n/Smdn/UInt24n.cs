// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET6_0_OR_GREATER
#define SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
#endif
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_MATH_CLAMP
#endif

#nullable enable

using System;

namespace Smdn;

internal class UInt24n {
  public static NotSupportedException CreateTypeIsNotConvertibleException<TUInt24n, TOther>()
    => new($"The type '{typeof(TOther)}' is not convertible to {typeof(TUInt24n).Name}");

  public static ArgumentException CreateArgumentIsNotComparableException<TUInt24n>(object? obj, string paramName)
    => new($"The value '{obj}'({obj?.GetType()}) is not comparable with {typeof(TUInt24n).Name}", paramName);

  public static OverflowException CreateOverflowException<TUInt24n>(object value)
    => new($"The value '{value}' is out of the range where the {typeof(TUInt24n).Name} can represent.");

  public static ReadOnlySpan<byte> ValidateAndGetSpan(byte[]? value, int startIndex, string paramName, int expectedSize)
  {
    if (value == null)
      throw new ArgumentNullException(paramName);
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(paramName, startIndex);
    if (value.Length - expectedSize < startIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(paramName, value, startIndex, expectedSize);

    return value.AsSpan(startIndex, expectedSize);
  }

#if !SYSTEM_MATH_CLAMP
  public static uint Clamp(uint value, uint min, uint max)
    => value < min ? min
      : value > max ? max
      : value;

  public static ulong Clamp(ulong value, ulong min, ulong max)
    => value < min ? min
      : value > max ? max
      : value;
#endif

#if !SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  public static (uint Quotient, uint Remainder) DivRem(uint left, uint right)
    => (left / right, left % right); // TODO: optimize

  public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
    => (left / right, left % right); // TODO: optimize
#endif

  public static int RegularizeShiftAmount(int shiftAmount, int maxShiftAmount)
  {
    if (shiftAmount == 0)
      return 0;

    shiftAmount %= maxShiftAmount;

    return 0 <= shiftAmount ? shiftAmount : maxShiftAmount + shiftAmount;
  }

  public static int RegularizeRotateAmount(int rotateAmount, int maxRotateAmount)
    => rotateAmount % maxRotateAmount;
}
