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
using System.Security.Cryptography;

namespace Smdn.Formats {
  // RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
  // 5.1.3. Mailbox International Naming Convention
  // http://tools.ietf.org/html/rfc3501#section-5.1.3
  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform()
      : base()
    {
    }

    public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode whitespaces)
      : base(whitespaces)
    {
    }

    private byte[] ReplaceInput(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      var replaced = new byte[inputCount];

      Buffer.BlockCopy(inputBuffer, inputOffset, replaced, 0, inputCount);

      // "," is used instead of "/"
      for (var i = 0; i < inputCount; i++) {
        if (replaced[i] == 0x2c)
          // replace ',' 0x2c to '/' 0x2f
          replaced[i] = 0x2f;
      }

      return replaced;
    }

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      return base.TransformBlock(ReplaceInput(inputBuffer, inputOffset, inputCount), 0, inputCount, outputBuffer, outputOffset);
    }

    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      return base.TransformFinalBlock(ReplaceInput(inputBuffer, inputOffset, inputCount), 0, inputCount);
    }
  }
}
