// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2010 smdn
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

namespace Smdn.Security.Cryptography {
  public static class ICryptoTransformExtensions {
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer)
    {
      if (transform == null)
        throw new ArgumentNullException("transform");
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");

      var outputBuffer = new byte[inputBuffer.Length * transform.OutputBlockSize];
      var outputOffset = 0;
      var inputOffset  = 0;

      if (transform.CanTransformMultipleBlocks) {
        var inputCount = (inputBuffer.Length / transform.InputBlockSize) * transform.InputBlockSize;

        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        inputOffset  += inputCount;
      }

      var inputRemain = inputBuffer.Length - inputOffset;

      while (transform.InputBlockSize <= inputRemain) {
        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, transform.InputBlockSize, outputBuffer, outputOffset);

        inputOffset += transform.InputBlockSize;
        inputRemain -= transform.InputBlockSize;
      }

      if (0 < inputRemain) {
        var finalBlock = transform.TransformFinalBlock(inputBuffer, inputOffset, inputBuffer.Length - inputOffset);

        if (outputBuffer.Length != outputOffset + finalBlock.Length)
          Array.Resize(ref outputBuffer, outputOffset + finalBlock.Length);

        Buffer.BlockCopy(finalBlock, 0, outputBuffer, outputOffset, finalBlock.Length);
      }
      else {
        if (outputBuffer.Length != outputOffset)
          Array.Resize(ref outputBuffer, outputOffset);
      }

      return outputBuffer;
    }
  }
}
