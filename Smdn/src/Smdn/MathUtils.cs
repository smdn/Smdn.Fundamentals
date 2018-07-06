// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Security.Cryptography;
using System.Threading;

namespace Smdn {
  public static class MathUtils {
    /// <summary>
    /// length of the hypotenuse of a triangle
    /// </summary>
    public static float Hypot(float x, float y)
    {
      return (float)Math.Sqrt((double)(x * x + y * y));
    }

    /// <summary>
    /// length of the hypotenuse of a triangle
    /// </summary>
    public static double Hypot(double x, double y)
    {
      return Math.Sqrt(x * x + y * y);
    }

    /// <summary>
    /// greatest common divisor of m and n
    /// </summary>
    public static int Gcd(int m, int n)
    {
      return (int)Gcd((long)m, (long)n);
    }

    /// <summary>
    /// greatest common divisor of m and n
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
    /// least common multiple of m and n
    /// </summary>
    public static int Lcm(int m, int n)
    {
      return (int)Lcm((long)m, (long)n);
    }

    /// <summary>
    /// least common multiple of m and n
    /// </summary>
    public static long Lcm(long m, long n)
    {
      return (m * n) / Gcd(m, n);
    }

#if !(NET || NETSTANDARD2_0)
    public static int DivRem(int a, int b, out int result)
    {
      int quotient = a / b;

      result = a - b * quotient;

      return quotient;
    }

    public static long DivRem(long a, long b, out long result)
    {
      long quotient = a / b;

      result = a - b * quotient;

      return quotient;
    }
#endif

    private static readonly long[] primeNumbers = new long[] {
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
        foreach (var p in primeNumbers) {
          if (n == p)
            return true;
          else if (n % p == 0L)
            return false;
        }

        for (var i = primeNumbers[primeNumbers.Length - 1]; i * i <= n; i += 2L) {
          if ((n % i) == 0L)
            return false;
        }

        return true;
      }
    }

    public static long NextPrimeNumber(long n)
    {
      foreach (var p in primeNumbers) {
        if (n < p)
          return p;
      }

      for (;;) {
        var i = 2L;

        n++;

        for (; i * i <= n && n % i != 0L;) {
          i = NextPrimeNumber(i);
        }

        if (n < i * i)
          return n;
      }
    }

    public static byte[] GetRandomBytes(int length)
    {
      var bytes = new byte[length];

      GetRandomBytes(bytes);

      return bytes;
    }

    private static RandomNumberGenerator defaultRng = null;

    public static void GetRandomBytes(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      LazyInitializer.EnsureInitialized(ref defaultRng, () => RandomNumberGenerator.Create());

      lock (defaultRng) {
        defaultRng.GetBytes(bytes);
      }
    }
  }
}
