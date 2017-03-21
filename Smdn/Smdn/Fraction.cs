// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
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

namespace Smdn {
  [Obsolete("This structure is deprecated.", true)]
  public struct Fraction :
    IEquatable<Fraction>,
    IEquatable<double>
  {
    public static readonly Fraction One   = new Fraction(1, 1);
    public static readonly Fraction Zero  = new Fraction(0, 1);
    public static readonly Fraction Empty = new Fraction();

    public long Numerator;
    public long Denominator;

    public Fraction(long numerator, long denominator)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

#region "number * fraction, fraction * number"
    public static int operator* (int number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static int operator* (Fraction fraction, int number)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static long operator* (long number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static long operator* (Fraction fraction, long number)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static double operator* (double number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static double operator* (Fraction fraction, double number)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static Fraction operator* (Fraction a, Fraction b)
    {
      throw new NotSupportedException("This member is deprecated.");
    }
#endregion

#region "number / fraction"
    public static int operator/ (int number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static long operator/ (long number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static double operator/ (double number, Fraction fraction)
    {
      throw new NotSupportedException("This member is deprecated.");
    }
#endregion

#region "fraction / number"
    public static Fraction operator/ (Fraction fraction, int number)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static Fraction operator/ (Fraction fraction, long number)
    {
      throw new NotSupportedException("This member is deprecated.");
    }
#endregion

#region "conversion"
    public static explicit operator int (Fraction frac)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static explicit operator long (Fraction frac)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public static explicit operator double (Fraction frac)
    {
      throw new NotSupportedException("This member is deprecated.");
    }
#endregion

    public override bool Equals(object other)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public bool Equals(double other)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public bool Equals(Fraction frac)
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public int ToInt32()
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public long ToInt64()
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public double ToDouble()
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public override int GetHashCode()
    {
      throw new NotSupportedException("This member is deprecated.");
    }

    public override string ToString()
    {
      throw new NotSupportedException("This member is deprecated.");
    }
  }
}
