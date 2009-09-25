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
using System.Runtime.InteropServices;

namespace Smdn.Mathematics {
  [CLSCompliant(false), StructLayout(LayoutKind.Explicit, Pack = 1)]
  public unsafe struct Vector4D {
    [FieldOffset( 0)] public fixed float Vector[4];

    [FieldOffset( 0)] public float X;
    [FieldOffset( 4)] public float Y;
    [FieldOffset( 8)] public float Z;
    [FieldOffset(12)] public float W;

    public Vector4D(float[] vector)
    {
      X = vector[0];
      Y = vector[1];
      Z = vector[2];
      W = vector[3];
    }

    public Vector4D(float x, float y, float z)
    {
      X = x;
      Y = y;
      Z = z;
      W = 1.0f;
    }

    internal Vector4D(float x, float y, float z, float w)
    {
      X = x;
      Y = y;
      Z = z;
      W = w;
    }

    public static Vector4D operator +(Vector4D v)
    {
      return new Vector4D(
                          v.X,
                          v.Y,
                          v.Z,
                          v.W
                          );
    }

    public static Vector4D operator -(Vector4D v)
    {
      return new Vector4D(
                          -v.X,
                          -v.Y,
                          -v.Z,
                          -v.W
                          );
    }

    public static Vector4D operator +(Vector4D v1, Vector4D v2)
    {
      return new Vector4D(
                          v1.X + v2.X,
                          v1.Y + v2.Y,
                          v1.Z + v2.Z,
                          v1.W + v2.W
                          );
    }

    public static Vector4D operator -(Vector4D v1, Vector4D v2)
    {
      return new Vector4D(
                          v1.X - v2.X,
                          v1.Y - v2.Y,
                          v1.Z - v2.Z,
                          v1.W - v2.W
                          );
    }

    public static Vector4D operator *(float scalar, Vector4D v)
    {
      return new Vector4D(
                          scalar * v.X,
                          scalar * v.Y,
                          scalar * v.Z,
                          scalar * v.W
                          );
    }

    public static Vector4D operator *(Vector4D v, float scalar)
    {
      return new Vector4D(
                          scalar * v.X,
                          scalar * v.Y,
                          scalar * v.Z,
                          scalar * v.W
                          );
    }

    public static Vector4D operator /(Vector4D v, float divisor)
    {
      float scalar = 1.0f / divisor;

      return new Vector4D(
                          scalar * v.X,
                          scalar * v.Y,
                          scalar * v.Z,
                          scalar * v.W
                          );
    }

    public override string ToString()
    {
      return string.Format( "{{{0:F4} {1:F4} {2:F4} {3:F4}}}", X, Y, Z, W);
    }
  }
}
