// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2013 smdn
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

namespace Smdn.Formats {
  // RFC 2152 - UTF-7 A Mail-Safe Transformation Format of Unicode
  // http://tools.ietf.org/html/rfc2152
  public class FromRFC2152ModifiedBase64Transform : FromBase64Transform, ICryptoTransform {
    private static readonly byte[] paddingBuffer = new byte[] {0x3d, 0x3d}; // '=' 0x3d

    public new int InputBlockSize {
      get { return 4; }
    }

    public FromRFC2152ModifiedBase64Transform()
      : base()
    {
    }

    public FromRFC2152ModifiedBase64Transform(FromBase64TransformMode whitespaces)
      : base(whitespaces)
    {
    }

    public virtual new int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      count += inputCount;

      return base.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
    }

    public virtual new byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      // The pad character "=" is not used when encoding
      // Modified Base64 because of the conflict with its use as an escape
      // character for the Q content transfer encoding in RFC 2047 header
      // fields, as mentioned above.
      var paddingCount = 4 - (count + inputCount) & 3;

      count = 0; // initialize

      switch (paddingCount) {
        case 1:
        case 2:
          var paddedInputBuffer = new byte[inputCount + paddingCount];

          Buffer.BlockCopy(inputBuffer, inputOffset, paddedInputBuffer, 0, inputCount);
          Buffer.BlockCopy(paddingBuffer, 0, paddedInputBuffer, inputCount, paddingCount);

          return base.TransformFinalBlock(paddedInputBuffer, 0, paddedInputBuffer.Length);

        case 3:
          throw new FormatException("incorrect form");

        default: // case 4
          return new byte[] {};
      }
    }

    private int count = 0;
  }
}
