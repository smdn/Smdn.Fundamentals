// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2011 smdn
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
      if (denominator == 0)
        throw new DivideByZeroException("denominator must be non-zero value");

      this.Numerator = numerator;
      this.Denominator = denominator;
    }

#region "number * fraction, fraction * number"
    public static int operator* (int number, Fraction fraction)
    {
      return (int)((number * fraction.Numerator) / fraction.Denominator);
    }

    public static int operator* (Fraction fraction, int number)
    {
      return (int)((number * fraction.Numerator) / fraction.Denominator);
    }

    public static long operator* (long number, Fraction fraction)
    {
      return (number * fraction.Numerator) / fraction.Denominator;
    }

    public static long operator* (Fraction fraction, long number)
    {
      return (number * fraction.Numerator) / fraction.Denominator;
    }

    public static double operator* (double number, Fraction fraction)
    {
      return (number * fraction.Numerator) / (double)fraction.Denominator;
    }

    public static double operator* (Fraction fraction, double number)
    {
      return (number * fraction.Numerator) / (double)fraction.Denominator;
    }

    public static Fraction operator* (Fraction a, Fraction b)
    {
      return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }
#endregion

#region "number / fraction"
    public static int operator/ (int number, Fraction fraction)
    {
      return (int)((number * fraction.Denominator) / fraction.Numerator);
    }

    public static long operator/ (long number, Fraction fraction)
    {
      return (number * fraction.Denominator) / fraction.Numerator;
    }

    public static double operator/ (double number, Fraction fraction)
    {
      return (number * fraction.Denominator) / (double)fraction.Numerator;
    }
#endregion

#region "fraction / number"
    public static Fraction operator/ (Fraction fraction, int number)
    {
      return new Fraction(fraction.Numerator, fraction.Denominator * number);
    }

    public static Fraction operator/ (Fraction fraction, long number)
    {
      return new Fraction(fraction.Numerator, fraction.Denominator * number);
    }
#endregion

#region "conversion"
    public static explicit operator int (Fraction frac)
    {
      return frac.ToInt32();
    }

    public static explicit operator long (Fraction frac)
    {
      return frac.ToInt64();
    }

    public static explicit operator double (Fraction frac)
    {
      return frac.ToDouble();
    }
#endregion

    public override bool Equals(object other)
    {
      if (other is double)
        return Equals((double)other);
      else if (other is Fraction)
        return Equals((Fraction)other);
      else
        return false;
    }

    public bool Equals(double other)
    {
      return (other == ToDouble());
    }

    public bool Equals(Fraction frac)
    {
      return (frac.Numerator == this.Numerator && frac.Denominator == this.Denominator);
    }

    public int ToInt32()
    {
      return (int)(Numerator / Denominator);
    }

    public long ToInt64()
    {
      return Numerator / Denominator;
    }

    public double ToDouble()
    {
      return Numerator / (double)Denominator;
    }

    public override int GetHashCode()
    {
      return (int)(Numerator ^ Denominator);
    }

    public override string ToString()
    {
      return string.Format("{0}/{1}", Numerator, Denominator);
    }
  }
}
