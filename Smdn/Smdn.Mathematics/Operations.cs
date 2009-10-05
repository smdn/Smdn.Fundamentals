// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn.Mathematics {
  public class Operations {
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
      return Math.Sqrt(x * x, y * y);
    }

    /// <summary>
    /// greatest common divisor of m and n
    /// </summary>
    public static int GCD(int m, int n)
    {
      return (int)GCD((long)m, (long)n);
    }

    /// <summary>
    /// greatest common divisor of m and n
    /// </summary>
    public static long GCD(long m, long n)
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
    public static int LCM(int m, int n)
    {
      return (int)LCM((long)m, (long)n);
    }

    /// <summary>
    /// least common multiple of m and n
    /// </summary>
    public static long LCM(long m, long n)
    {
      return (m * n) / GCD(m, n);
    }
  }
}
