// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
using ToBase64Transform = System.Security.Cryptography.ToBase64Transform;
#else
using ToBase64Transform = Smdn.Security.Cryptography.ToBase64Transform;
#endif

namespace Smdn.Formats.ModifiedBase64 {
  // RFC 2152 - UTF-7 A Mail-Safe Transformation Format of Unicode
  // http://tools.ietf.org/html/rfc2152
  public class ToRFC2152ModifiedBase64Transform : ICryptoTransform {
    public bool CanReuseTransform => true;
    public bool CanTransformMultipleBlocks => false;
    public int InputBlockSize => 3;
    public int OutputBlockSize => 4;

    public ToRFC2152ModifiedBase64Transform()
    {
      this.toBase64Transform = Base64.CreateToBase64Transform();
    }

    public void Dispose()
    {
      toBase64Transform?.Dispose();
      toBase64Transform = null;
    }

    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (toBase64Transform == null)
        throw new ObjectDisposedException(GetType().FullName);

      return toBase64Transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
    }

    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (toBase64Transform == null)
        throw new ObjectDisposedException(GetType().FullName);

      // The pad character "=" is not used when encoding
      // Modified Base64 because of the conflict with its use as an escape
      // character for the Q content transfer encoding in RFC 2047 header
      // fields, as mentioned above.
      var transformed = toBase64Transform.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

      int padding = -1;

      for (var i = transformed.Length - 1; 0 <= i; i--) {
        if (transformed[i] == 0x3d) // '=' 0x3d
          padding = i;
        else
          break;
      }

      if (0 <= padding)
        Array.Resize(ref transformed, padding);

      return transformed;
    }

    private ICryptoTransform toBase64Transform;
  }
}
