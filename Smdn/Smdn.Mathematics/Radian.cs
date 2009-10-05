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
  public struct Radian :
    IComparable<Radian>,
    IComparable<float>,
    IEquatable<Radian>,
    IEquatable<float>
  {
    public static readonly Radian Zero = new Radian(0.0f);
    public static readonly Radian One = new Radian(1.0f);
    public static readonly Radian PI = new Radian((float)Math.PI);

    public static readonly Radian RightAngle = Radian.PI * 0.5f;
    public static readonly Radian StraightAngle = Radian.PI;
    public static readonly Radian FullAngle = Radian.PI * 2.0f;

    public float Value;

    public Radian Regularized {
      get
      {
        var val = Value;

        // XXX
        if (val < 0.0f) {
          for (;;) {
            if (0.0f <= val)
              return new Radian(val);
            else
              val += FullAngle.Value;
          }
        }
        else {
          for (;;) {
            if (val < FullAngle.Value)
              return new Radian(val);
            else
              val -= FullAngle.Value;
          }
        }
      }
    }

    public int Quadrant {
      // XXX
      get { return (int)(Regularized / RightAngle) + 1; }
    }

    public Radian(float @value)
    {
      this.Value = value;
    }

#region "conversion"
    public static Radian FromDegree(float degree)
    {
      return (Radian.PI * degree) / 180.0f;
    }

    public float ToDegree()
    {
      return (this / Radian.PI) * 180.0f;
    }

    public float ToSingle()
    {
      return Value;
    }

    public static explicit operator float(Radian rad)
    {
      return rad.Value;
    }

    public static explicit operator Radian(float val)
    {
      return new Radian(val);
    }
#endregion

#region "operation"
    public static Radian operator + (Radian x, Radian y)
    {
      return new Radian(x.Value + y.Value);
    }

    public static Radian operator - (Radian x, Radian y)
    {
      return new Radian(x.Value - y.Value);
    }

    public static Radian operator * (float x, Radian y)
    {
      return new Radian(x * y.Value);
    }

    public static Radian operator * (Radian x, float y)
    {
      return new Radian(x.Value * y);
    }

    public static float operator / (Radian x, Radian y)
    {
      return x.Value / y.Value;
    }

    public static Radian operator / (Radian x, float y)
    {
      return new Radian(x.Value / y);
    }
#endregion

#region "comparison"
    public static bool operator < (Radian x, Radian y)
    {
      return (x.CompareTo(y) < 0);
    }

    public static bool operator <= (Radian x, Radian y)
    {
      return (x.CompareTo(y) <= 0);
    }

    public static bool operator > (Radian x, Radian y)
    {
      return y < x;
    }

    public static bool operator >= (Radian x, Radian y)
    {
      return y <= x;
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else if (obj is Radian)
        return CompareTo((Radian)obj);
      else if (obj is float)
        return CompareTo((float)obj);
      else
        throw new ArgumentException("ojb is not Radian");
    }

    public int CompareTo(float other)
    {
      return Value.CompareTo(other);
    }

    public int CompareTo(Radian other)
    {
      return this.Value.CompareTo(other.Value);
    }
#endregion

#region "equality"
    public static bool operator == (Radian x, Radian y)
    {
      return x.Value == y.Value;
    }

    public static bool operator != (Radian x, Radian y)
    {
      return x.Value == y.Value;
    }

    public override bool Equals(object obj)
    {
      if (obj is Radian)
        return Equals((Radian)obj);
      else if (obj is float)
        return Equals((float)obj);
      else
        return false;
    }

    public bool Equals(Radian other)
    {
      return this.Value == other.Value;
    }

    public bool Equals(float other)
    {
      return this.Value == other;
    }
#endregion

    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("{0} [rad]", Value);
    }
  }
}
