// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2014 smdn
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
using System.Text;

namespace Smdn.Security.Cryptography {
  public static class ICryptoTransformExtensions {
    public static string TransformStringTo(this ICryptoTransform transform, string str, Encoding encoding)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      var bytes = encoding.GetBytes(str);

      return Encoding.ASCII.GetString(TransformBytes(transform, bytes, 0, bytes.Length));
    }

    public static string TransformStringFrom(this ICryptoTransform transform, string str, Encoding encoding)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      var bytes = Encoding.ASCII.GetBytes(str);

      return encoding.GetString(TransformBytes(transform, bytes, 0, bytes.Length));
    }

    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer)
    {
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");

      return TransformBytes(transform, inputBuffer, 0, inputBuffer.Length);
    }

    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (transform == null)
        throw new ArgumentNullException("transform");

      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputOffset", inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);

      var outputBuffer = new byte[inputCount * transform.OutputBlockSize];
      var outputOffset = 0;

      if (transform.CanTransformMultipleBlocks) {
        var bytesToTransform = (inputCount / transform.InputBlockSize) * transform.InputBlockSize;

        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, bytesToTransform, outputBuffer, outputOffset);
        inputOffset  += bytesToTransform;
        inputCount   -= bytesToTransform;
      }

      var inputBlockSize = transform.InputBlockSize;

      while (inputBlockSize <= inputCount) {
        outputOffset += transform.TransformBlock(inputBuffer, inputOffset, inputBlockSize, outputBuffer, outputOffset);

        inputOffset += inputBlockSize;
        inputCount  -= inputBlockSize;
      }

      var finalBlock = transform.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

      if (outputBuffer.Length != outputOffset + finalBlock.Length)
        Array.Resize(ref outputBuffer, outputOffset + finalBlock.Length);

      Buffer.BlockCopy(finalBlock, 0, outputBuffer, outputOffset, finalBlock.Length);

      return outputBuffer;
    }
  }
}
