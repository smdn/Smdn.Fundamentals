// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Security.Cryptography;

#if !SYSTEM_MATH_DIVREM
internal sealed class MathDivRemShim {
  public static int DivRem(int a, int b, out int result)
  {
    var quotient = a / b;

    result = a - (b * quotient);

    return quotient;
  }
}
#endif
