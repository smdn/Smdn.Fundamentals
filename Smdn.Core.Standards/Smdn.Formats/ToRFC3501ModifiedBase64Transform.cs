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
  // RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
  // 5.1.3. Mailbox International Naming Convention
  // http://tools.ietf.org/html/rfc3501#section-5.1.3
  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform, ICryptoTransform {
    public ToRFC3501ModifiedBase64Transform()
      : base()
    {
    }

    private void ReplaceOutput(byte[] buffer, int offset, int count)
    {
      // "," is used instead of "/"
      while (0 < count--) {
        if (buffer[offset] == 0x2f)
          // replace '/' 0x2f to ',' 0x2c
          buffer[offset] = 0x2c;
        offset++;
      }
    }

    public new int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      var outputCount = base.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);

      ReplaceOutput(outputBuffer, outputOffset, outputCount);

      return outputCount;
    }

    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      var outputBuffer = base.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

      ReplaceOutput(outputBuffer, 0, outputBuffer.Length);

      return outputBuffer;
    }
  }
}
