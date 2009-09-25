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
  public unsafe struct Matrix4D {
    [FieldOffset( 0)] public fixed float Matrix[16];

    [FieldOffset( 0)] public float E11;
    [FieldOffset( 4)] public float E12;
    [FieldOffset( 8)] public float E13;
    [FieldOffset(12)] public float E14;

    [FieldOffset(16)] public float E21;
    [FieldOffset(20)] public float E22;
    [FieldOffset(24)] public float E23;
    [FieldOffset(28)] public float E24;

    [FieldOffset(32)] public float E31;
    [FieldOffset(36)] public float E32;
    [FieldOffset(40)] public float E33;
    [FieldOffset(44)] public float E34;

    [FieldOffset(48)] public float E41;
    [FieldOffset(52)] public float E42;
    [FieldOffset(56)] public float E43;
    [FieldOffset(60)] public float E44;

    public float Determinant {
      get
      {
        var det = 0.0f;

        fixed (float* mat = Matrix) {
          // x & 0xf => x % 16
          for (var x = 3; x <= 15; x += 4) {
            //       +1 +5 +5  %16
            //    03 04 09 14  =>  03 04 09 14  ->           03 - 04 09 14
            // +4 07 08 13 18  =>  07 08 13 02  ->        02 07 - 08 13
            // +4 11 12 17 23  =>  11 12 01 06  ->     01 06 11 - 12
            // +4 15 16 21 26  =>  15 00 05 10  ->  00 05 10 15 -
            det += (mat[x] * mat[(x + 1) & 0xf] * mat[(x + 6) & 0xf] * mat[(x + 11) & 0xf]);
          }

          for (var x = 7; x <= 19; x += 4) {
            //       +3 +3 +3  %16
            //    07 10 13 16  =>  07 10 13 00  ->           00 - 07 10 13
            // +4 11 14 17 20  =>  11 14 01 04  ->        01 04 - 11 14
            // +4 15 18 21 24  =>  15 02 05 08  ->     02 05 08 - 15
            // +4 19 22 25 28  =>  03 06 09 12  ->  03 06 09 12 -
            det -= (mat[x & 0xf] * mat[(x + 3) & 0xf] * mat[(x + 6) & 0xf] * mat[(x + 9) & 0xf]);
          }
        }

        return det;
      }
    }

    public Matrix4D Transposed {
      get
      {
        return new Matrix4D(new float[] {
          E11, E21, E31, E41,
          E12, E22, E32, E42,
          E13, E23, E33, E43,
          E14, E24, E34, E44});
      }
    }

    private Matrix4D(float[] matrix)
    {
      var index = 0;

      E11 = matrix[index++];
      E12 = matrix[index++];
      E13 = matrix[index++];
      E14 = matrix[index++];

      E21 = matrix[index++];
      E22 = matrix[index++];
      E23 = matrix[index++];
      E24 = matrix[index++];

      E31 = matrix[index++];
      E32 = matrix[index++];
      E33 = matrix[index++];
      E34 = matrix[index++];

      E41 = matrix[index++];
      E42 = matrix[index++];
      E43 = matrix[index++];
      E44 = matrix[index++];
    }

    public static Matrix4D E()
    {
      return new Matrix4D(new float[] {
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateX(double rad)
    {
      return new Matrix4D(new float[] {
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, (float) Math.Cos(rad), (float) Math.Sin(rad), 0.0f,
        0.0f, (float)-Math.Sin(rad), (float) Math.Cos(rad), 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateX(double y, double z)
    {
      var r = 1.0f / Math.Sqrt((y * y) + (z * z));

      return new Matrix4D(new float[] {
        1.0f, 0.0f, 0.0f, 0.0f,
        0.0f, (float)( z * r), (float)( y * r), 0.0f,
        0.0f, (float)(-y * r), (float)( z * r), 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateY(double rad)
    {
      return new Matrix4D(new float[] {
        (float) Math.Cos(rad), 0.0f, (float) Math.Sin(rad), 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        (float)-Math.Sin(rad), 0.0f, (float) Math.Cos(rad), 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateY(double x, double z)
    {
      var r = 1.0 / Math.Sqrt((x * x) + (z * z));

      return new Matrix4D(new float[] {
        (float)( z * r), 0.0f, (float)( x * r), 0.0f,
        0.0f, 1.0f, 0.0f, 0.0f,
        (float)(-x * r), 0.0f, (float)( z * r), 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateZ(double rad)
    {
      return new Matrix4D(new float[] {
        (float) Math.Cos(rad), (float) Math.Sin(rad), 0.0f, 0.0f,
        (float)-Math.Sin(rad), (float) Math.Cos(rad), 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D RotateZ(double x, double y)
    {
      var r = 1.0 / Math.Sqrt((x * x) + (y * y));

      return new Matrix4D(new float[] {
        (float)( x * r), (float)( y * r), 0.0f, 0.0f,
        (float)(-y * r), (float)( x * r), 0.0f, 0.0f,
        0.0f, 0.0f, 1.0f, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D Scale(float scale)
    {
      return new Matrix4D(new float[] {
        scale,  0.0f,  0.0f, 0.0f,
         0.0f, scale,  0.0f, 0.0f,
         0.0f,  0.0f, scale, 0.0f,
         0.0f,  0.0f,  0.0f, 1.0f});
    }

    public static Matrix4D Scale(float x, float y, float z)
    {
      return new Matrix4D(new float[] {
           x, 0.0f, 0.0f, 0.0f,
        0.0f,    y, 0.0f, 0.0f,
        0.0f, 0.0f,    z, 0.0f,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D Shift(float x, float y, float z)
    {
      return new Matrix4D(new float[] {
        1.0f, 0.0f, 0.0f, x,
        0.0f, 1.0f, 0.0f, y,
        0.0f, 0.0f, 1.0f, z,
        0.0f, 0.0f, 0.0f, 1.0f});
    }

    public static Matrix4D Shift(Vector4D vector)
    {
      return new Matrix4D(new float[] {
        1.0f, 0.0f, 0.0f, vector.X,
        0.0f, 1.0f, 0.0f, vector.Y,
        0.0f, 0.0f, 1.0f, vector.Z,
        0.0f, 0.0f, 0.0f, vector.W});
    }

    public static Matrix4D operator +(Matrix4D matrix)
    {
      var index = 0;

#if false // CS0213 with csc.exe 3.5
      fixed (float* mat = matrix.Matrix) {
        return new Matrix4D(new[] {
          mat[index++],
          mat[index++],
          mat[index++],
          mat[index++],

          mat[index++],
          mat[index++],
          mat[index++],
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
#else
      return new Matrix4D(new[] {
        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],

        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],

        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],

        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],
        matrix.Matrix[index++],
      });
#endif
    }

    public static Matrix4D operator -(Matrix4D matrix)
    {
      var index = 0;

#if false // CS0213 with csc.exe 3.5
      fixed (float* mat = matrix.Matrix) {
        return new Matrix4D(new[] {
          -mat[index++],
          -mat[index++],
          -mat[index++],
          -mat[index++],

          -mat[index++],
          -mat[index++],
          -mat[index++],
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
#else
      return new Matrix4D(new[] {
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],

        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],

        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],

        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
        -matrix.Matrix[index++],
      });
#endif
    }

    public static Matrix4D operator +(Matrix4D matrix1, Matrix4D matrix2)
    {
      var index = 0;

#if false // CS0213 with csc.exe 3.5
      fixed (float* mat1 = matrix1.Matrix, mat2 = matrix2.Matrix) {
        return new Matrix4D(new[] {
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],

          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
          mat1[index] + mat2[index++],
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
#else
      return new Matrix4D(new[] {
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],

        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],

        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],

        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
        matrix1.Matrix[index] + matrix2.Matrix[index++],
      });
#endif
    }

    public static Matrix4D operator *(float scalar, Matrix4D matrix)
    {
      var index = 0;

#if false // CS0213 with csc.exe 3.5
      fixed (float* mat = matrix.Matrix) {
        return new Matrix4D(new[] {
          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],

          scalar * mat[index++],
          scalar * mat[index++],
          scalar * mat[index++],
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
#else
      return new Matrix4D(new[] {
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],

        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],

        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],

        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
        scalar * matrix.Matrix[index++],
      });
#endif
    }

    public static Vector4D operator *(Vector4D vector, Matrix4D matrix)
    {
      return new Vector4D(
                          vector.X * matrix.E11 + vector.Y * matrix.E12 + vector.Z * matrix.E13 + vector.W * matrix.E14,
                          vector.X * matrix.E21 + vector.Y * matrix.E22 + vector.Z * matrix.E23 + vector.W * matrix.E24,
                          vector.X * matrix.E31 + vector.Y * matrix.E32 + vector.Z * matrix.E33 + vector.W * matrix.E34,
                          vector.X * matrix.E41 + vector.Y * matrix.E42 + vector.Z * matrix.E43 + vector.W * matrix.E44
                          );
    }

    public static Matrix4D operator *(Matrix4D matrix1, Matrix4D matrix2)
    {
      var matrix = new Matrix4D();

#if false // CS0213 with csc.exe 3.5
      fixed (float* mat = matrix.Matrix, mat1 = matrix1.Matrix, mat2 = matrix2.Matrix) {
        var ij = 0;
        for (var i = 0; i < 4; i++) { // row
          for (var j = 0; j < 4; j++) { // column
            var ik = i << 2;
            var kj = j;
            var sum = 0.0f;
            for (var k = 0; k < 4; k++) {
              sum += mat1[ik] * mat2[kj];
              ik += 1;
              kj += 4;
            }
            mat[ij++] = sum;
          }
        }
      }
#else
      var ij = 0;

      for (var i = 0; i < 4; i++) { // row
        for (var j = 0; j < 4; j++) { // column
          var ik = i << 2;
          var kj = j;
          var sum = 0.0f;
          for (var k = 0; k < 4; k++) {
            sum += matrix1.Matrix[ik] * matrix2.Matrix[kj];
            ik += 1;
            kj += 4;
          }
          matrix.Matrix[ij++] = sum;
        }
      }
#endif

      return matrix;
    }

    public override string ToString()
    {
      return string.Format("{{{0:F4} {1:F4} {2:F4} {3:F4}{4}", E11, E12, E13, E14, Environment.NewLine) +
              string.Format( " {0:F4} {1:F4} {2:F4} {3:F4}{4}", E21, E22, E23, E24, Environment.NewLine) +
              string.Format( " {0:F4} {1:F4} {2:F4} {3:F4}{4}", E31, E32, E33, E34, Environment.NewLine) +
              string.Format( " {0:F4} {1:F4} {2:F4} {3:F4}}}",  E41, E42, E43, E44);
    }
  }
}
