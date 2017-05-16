// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2017 smdn
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

#if !(NET46 || NETSTANDARD20)
using System;
using System.Security.Cryptography;

namespace Smdn.Security.Cryptography {
  public class ToBase64Transform : ICryptoTransform {
    public bool CanReuseTransform => true;
    public bool CanTransformMultipleBlocks => false;
    public int InputBlockSize => 3;
    public int OutputBlockSize => 4;

    public ToBase64Transform()
    {
    }

    public void Dispose()
    {
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputOffset", inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);
      if (inputCount < InputBlockSize)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThan(InputBlockSize, "inputCount", inputCount);

      if (outputBuffer == null)
        throw new ArgumentNullException("outputBuffer");
      if (outputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("outputOffset", outputOffset);
      if (outputBuffer.Length <= outputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("outputOffset", outputBuffer, outputOffset, outputBuffer.Length);
      if (outputBuffer.Length < outputOffset + OutputBlockSize)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("outputOffset + OutputBlockSize", outputBuffer, outputOffset, outputBuffer.Length);

      return UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
    }

    private static int UncheckedTransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      const byte padding = 0x3d; // '='

      var ret = 0;
      int b = 0;

      b = ((int)inputBuffer[inputOffset] & 0xfc) >> 2;

      outputBuffer[outputOffset++] = toBase64Table[b];
      ret++;

      b = ((int)inputBuffer[inputOffset++] & 0x03 ) << 4;
      inputCount--;

      if (0 < inputCount) {
        b |= ((int)inputBuffer[inputOffset] & 0xf0) >> 4;

        outputBuffer[outputOffset++] = toBase64Table[b];
        ret++;

        b = ((int)inputBuffer[inputOffset++] & 0x0f) << 2;
        inputCount--;

        if (0 < inputCount) {
          b |= ((int)inputBuffer[inputOffset] & 0xc0) >> 6;

          outputBuffer[outputOffset++] = toBase64Table[b];
          ret++;

          b = ((int)inputBuffer[inputOffset++] & 0x3f);
          inputCount--;

          outputBuffer[outputOffset++] = toBase64Table[b];
          ret++;
        }
        else {
          outputBuffer[outputOffset++] = toBase64Table[b];
          outputBuffer[outputOffset++] = padding;
          ret += 2;
        }
      }
      else {
        outputBuffer[outputOffset++] = toBase64Table[b];
        outputBuffer[outputOffset++] = padding;
        outputBuffer[outputOffset++] = padding;
        ret += 3;
      }

      return ret;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputOffset", inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);
      if (InputBlockSize < inputCount)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(InputBlockSize, "inputCount", inputCount);

      if (inputCount == 0)
        return Array.Empty<byte>();

      var ret = new byte[OutputBlockSize];
      var len = UncheckedTransformBlock(inputBuffer, inputOffset, inputCount, ret, 0);

      Array.Resize(ref ret, len);

      return ret;
    }

    private static readonly byte[] toBase64Table = new byte[] {
    /*0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,*/
      0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, // 0x00
      0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, // 0x10
      0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, // 0x20
      0x77, 0x78, 0x79, 0x7a, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x2b, 0x2f, // 0x30
    /*0x3d,*/
    };
  }
}
#endif