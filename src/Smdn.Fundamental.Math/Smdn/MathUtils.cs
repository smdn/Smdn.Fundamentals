// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0004

using System;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class MathUtils {
  /// <summary>
  /// length of the hypotenuse of a triangle.
  /// </summary>
  public static float Hypot(float x, float y)
    => (float)Math.Sqrt((double)((x * x) + (y * y)));

  /// <summary>
  /// length of the hypotenuse of a triangle.
  /// </summary>
  public static double Hypot(double x, double y) => Math.Sqrt((x * x) + (y * y));

  /// <summary>
  /// greatest common divisor of m and n.
  /// </summary>
  public static int Gcd(int m, int n) => (int)Gcd((long)m, (long)n);

  /// <summary>
  /// greatest common divisor of m and n.
  /// </summary>
  public static long Gcd(long m, long n)
  {
    long mm, nn;

    if (m < n) {
      mm = n;
      nn = m;
    }
    else {
      mm = m;
      nn = n;
    }

    while (nn != 0) {
      var t = mm % nn;
      mm = nn;
      nn = t;
    }

    return mm;
  }

  /// <summary>
  /// least common multiple of m and n.
  /// </summary>
  public static int Lcm(int m, int n) => (int)Lcm((long)m, (long)n);

  /// <summary>
  /// least common multiple of m and n.
  /// </summary>
  public static long Lcm(long m, long n) => m * n / Gcd(m, n);

#if !SYSTEM_MATH_DIVREM
  public static int DivRem(int a, int b, out int result)
  {
    int quotient = a / b;

    result = a - (b * quotient);

    return quotient;
  }

  public static long DivRem(long a, long b, out long result)
  {
    long quotient = a / b;

    result = a - (b * quotient);

    return quotient;
  }
#endif

  private static readonly long[] PrimeNumbers = new long[] {
    2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53,
    59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127,
    131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
    211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283,
    293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383,
    389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467,
    479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577,
    587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661,
    673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769,
    773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877,
    881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983,
    991, 997,
  };

  public static bool IsPrimeNumber(long n)
  {
    if (n <= 1L)
      return false; // XXX

    if ((n & 1) == 0L) {
      // n is multiple of 2
      return n == 2L;
    }
    else {
      foreach (var p in PrimeNumbers) {
        if (n == p)
          return true;
        else if (n % p == 0L)
          return false;
      }

      for (var i = PrimeNumbers[PrimeNumbers.Length - 1]; i * i <= n; i += 2L) {
        if ((n % i) == 0L)
          return false;
      }

      return true;
    }
  }

  public static long NextPrimeNumber(long n)
  {
    foreach (var p in PrimeNumbers) {
      if (n < p)
        return p;
    }

    for (; ; ) {
      var i = 2L;

      n++;

      for (; i * i <= n && n % i != 0L;) {
        i = NextPrimeNumber(i);
      }

      if (n < i * i)
        return n;
    }
  }

  [Obsolete("use Smdn.Nonce.GetRandomBytes instead", error: true)]
  public static byte[] GetRandomBytes(int length)
    => throw new NotImplementedException("use Smdn.Nonce.GetRandomBytes instead");

  [Obsolete("use Smdn.Nonce.GetRandomBytes instead", error: true)]
  public static void GetRandomBytes(byte[] bytes)
    => throw new NotImplementedException("use Smdn.Nonce.GetRandomBytes instead");
}
