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
  public struct Polar :
    IEquatable<Polar>
  {
    public static readonly Polar Zero = new Polar(0.0f, Radian.Zero);

    public float Radius {
      get; set;
    }

    public Radian Angle {
      get; set;
    }

    public Polar Regularized {
      get
      {
        var r = Radius;
        var a = Angle;

        if (r < 0.0f) {
          r = -r;
          a = a - Radian.PI;
        }

        return new Polar(r, a.Regularized);
      }
    }

    public float X {
      get { return (float)(Radius * Math.Cos((double)Angle)); }
    }

    public float Y {
      get { return (float)(Radius * Math.Sin((double)Angle)); }
    }

    public Polar(float radius, float angle)
      : this(radius, (Radian)angle)
    {
    }

    public Polar(float radius, Radian angle)
    {
      this.Radius = radius;
      this.Angle = angle;
    }

#region "conversion"
    public static Polar FromCartecian(float x, float y)
    {
      return new Polar(Operations.Hypot(x, y), new Radian((float)Math.Atan2(y, x)));
    }

    // System.Drawing.dll
    //public PointF ToPointF
    //public PointF
    //public static explicit operator PointF(Polar polar)
    //public static explicit operator Polar(PointF cartesian)
#endregion

#region "operation"
    public static Polar operator + (Polar pol)
    {
      return new Polar(pol.Radius, pol.Angle);
    }

    public static Polar operator - (Polar pol)
    {
      return new Polar(-pol.Radius, pol.Angle);
    }

    public static Polar operator * (float x, Polar y)
    {
      return new Polar(x * y.Radius, y.Angle);
    }

    public static Polar operator * (Polar x, float y)
    {
      return new Polar(x.Radius * y, x.Angle);
    }

    public static Polar operator * (Polar x, Polar y)
    {
      return new Polar(x.Radius * y.Radius, x.Angle + y.Angle);
    }

    public static Polar operator / (float x, Polar y)
    {
      return new Polar(x / y.Radius, -y.Angle);
    }

    public static Polar operator / (Polar x, float y)
    {
      return new Polar(x.Radius / y, x.Angle);
    }

    public static Polar operator / (Polar x, Polar y)
    {
      return new Polar(x.Radius / y.Radius, x.Angle - y.Angle);
    }
#endregion

#region "equality"
    public static bool operator == (Polar x, Polar y)
    {
      return x.Radius == y.Radius && x.Angle == y.Angle;
    }

    public static bool operator != (Polar x, Polar y)
    {
      return !(x == y);
    }

    public override bool Equals(object obj)
    {
      if (obj is Polar)
        return Equals((Polar)obj);
      else
        return false;
    }

    public bool Equals(Polar other)
    {
      return this == other;
    }
#endregion

    public override int GetHashCode()
    {
      return Radius.GetHashCode() ^ Angle.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("({0}, {1})", Radius, Angle);
    }
  }
}
