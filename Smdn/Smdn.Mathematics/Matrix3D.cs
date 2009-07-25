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
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public unsafe struct Matrix3D {
    [FieldOffset( 0)] public fixed float Matrix[9];

    [FieldOffset( 0)] public float E11;
    [FieldOffset( 4)] public float E12;
    [FieldOffset( 8)] public float E13;

    [FieldOffset(12)] public float E21;
    [FieldOffset(16)] public float E22;
    [FieldOffset(20)] public float E23;

    [FieldOffset(24)] public float E31;
    [FieldOffset(28)] public float E32;
    [FieldOffset(32)] public float E33;

    public float Determinant {
      get
      {
        return
          + E11 * E22 * E33 + E12 * E23 * E31 + E13 * E21 * E32
          - E11 * E23 * E32 - E12 * E21 * E33 - E13 * E22 * E31;
      }
    }

    public Matrix3D Transposed {
      get
      {
        return new Matrix3D(new float[] {
          E11, E21, E31,
          E12, E22, E32,
          E13, E23, E33});
      }
    }

    private Matrix3D(float[] matrix)
    {
      var index = 0;

      E11 = matrix[index++];
      E12 = matrix[index++];
      E13 = matrix[index++];

      E21 = matrix[index++];
      E22 = matrix[index++];
      E23 = matrix[index++];

      E31 = matrix[index++];
      E32 = matrix[index++];
      E33 = matrix[index++];
    }

    public static Matrix3D Create(float[] matrix)
    {
      return new Matrix3D(matrix);
    }

    public static Matrix3D E()
    {
      return new Matrix3D(new float[] {
        1.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 1.0f});
    }

    public static Matrix3D Rotate(double rad)
    {
      return new Matrix3D(new float[] {
        (float) Math.Cos(rad), (float) Math.Sin(rad), 0.0f,
        (float)-Math.Sin(rad), (float) Math.Cos(rad), 0.0f,
        0.0f, 0.0f, 1.0f});
    }

    public static Matrix3D Rotate(double y, double x)
    {
      var r = 1.0f / Math.Sqrt((x * x) + (y * y));

      return new Matrix3D(new float[] {
        (float)( x * r), (float)( y * r), 0.0f,
        (float)(-y * r), (float)( x * r), 0.0f,
        0.0f, 0.0f, 1.0f});
    }

    public static Matrix3D Scale(float scale)
    {
      return new Matrix3D(new float[] {
        scale,  0.0f, 0.0f,
         0.0f, scale, 0.0f,
         0.0f,  0.0f, 1.0f});
    }

    public static Matrix3D Scale(float x, float y)
    {
      return new Matrix3D(new float[] {
           x, 0.0f, 0.0f,
        0.0f,    y, 0.0f,
        0.0f, 0.0f, 1.0f});
    }

    public static Matrix3D Shift(float x, float y)
    {
      return new Matrix3D(new float[] {
        1.0f, 0.0f, x,
        0.0f, 1.0f, y,
        0.0f, 0.0f, 1.0f});
    }

    public static Matrix3D Shift(Vector3D vector)
    {
      return new Matrix3D(new float[] {
        1.0f, 0.0f, vector.X,
        0.0f, 1.0f, vector.Y,
        0.0f, 0.0f, vector.W});
    }

    public static Matrix3D operator +(Matrix3D matrix)
    {
      var index = 0;

      fixed (float* mat = matrix.Matrix) {
        return new Matrix3D(new[] {
          mat[index++],
          mat[index++],
          mat[index++],

          mat[index++],
          mat[index++],
          mat[index++],

          mat[index++],
          mat[index++],
          mat[index++],
        });
      }
    }

    public static Matrix3D operator -(Matrix3D matrix)
    {
      var index = 0;

      fixed (float* mat = matrix.Matrix) {
        return new Matrix3D(new[] {
          -mat[index++],
          -mat[index++],
          -mat[index++],

          -mat[index++],
          -mat[index++],
          -mat[index++],

          -mat[index++],
          -mat[index++],
          -mat[index++],
        });
      }
    }

    public static Matrix3D operator +(Matrix3D matrix1, Matrix3D matrix2)
    {
      var index = 0;

      fixed (float* mat1 = matrix1.Matrix, mat2 = matrix2.Matrix) {
        return new Matrix3D(new[] {
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],

          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],

          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
        });
      }
    }

    public static Matrix3D operator *(float scalar, Matrix3D matrix)
    {
      var index = 0;

      fixed (float* mat = matrix.Matrix) {
        return new Matrix3D(new[] {
          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],

          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],

          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],
        });
      }
    }

    public static Vector3D operator *(Vector3D vector, Matrix3D matrix)
    {
      return new Vector3D(
                          vector.X * matrix.E11 + vector.Y * matrix.E12 + vector.W * matrix.E13,
                          vector.X * matrix.E21 + vector.Y * matrix.E22 + vector.W * matrix.E23,
                          vector.X * matrix.E31 + vector.Y * matrix.E32 + vector.W * matrix.E33
                          );
    }

    public static Matrix3D operator *(Matrix3D matrix1, Matrix3D matrix2)
    {
      var matrix = new Matrix3D();

      fixed (float* mat = matrix.Matrix, mat1 = matrix1.Matrix, mat2 = matrix2.Matrix) {
        var ij = 0;
        for (var i = 0; i < 3; i++) { // row
          for (var j = 0; j < 3; j++) { // column
            var ik = i << 2;
            var kj = j;
            var sum = 0.0f;
            for (var k = 0; k < 3; k++) {
              sum += mat1[ik] * mat2[kj];
              ik += 1;
              kj += 4;
            }
            mat[ij++] = sum;
          }
        }
      }

      return matrix;
    }

    public override string ToString()
    {
      return string.Format("{{{0:F4} {1:F4} {2:F4}{3}", E11, E12, E13, Environment.NewLine) +
              string.Format( " {0:F4} {1:F4} {2:F4}{3}", E21, E22, E23, Environment.NewLine) +
              string.Format( " {0:F4} {1:F4} {2:F4}}}",  E31, E32, E33);
    }
  }
}
