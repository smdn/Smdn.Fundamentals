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

using System;
using System.Security.Cryptography;

namespace Smdn.Security.Cryptography {
  public class FromBase64Transform : ICryptoTransform {
    public bool CanReuseTransform => true;
    public bool CanTransformMultipleBlocks => false;
    public int InputBlockSize => 1;
    public int OutputBlockSize => 3;

    public FromBase64Transform()
      : this(FromBase64TransformMode.IgnoreWhiteSpaces)
    {
    }

    public FromBase64Transform(FromBase64TransformMode mode)
    {
      ignoreWhiteSpaces = (mode == FromBase64TransformMode.IgnoreWhiteSpaces);
    }

    public void Dispose()
    {
      disposed = true;
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);

      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputOffset", inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);

      if (outputBuffer == null)
        throw new ArgumentNullException("outputBuffer");
      if (outputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("outputOffset", outputOffset);
      if (outputBuffer.Length <= outputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("outputOffset", outputBuffer, outputOffset, outputBuffer.Length);

      var ret = 0;
      var padded = false;
      var decoded = new int[4];

      for(;;) {
        if (inputCount < 4)
          break;

        if (padded)
          throw new FormatException("incorrect padding");

        for (var b = 0; b < 4;) {
          if (inputCount <= 0)
            throw new FormatException("invalid format");

          var octet = inputBuffer[inputOffset++];

          inputCount--;

          if (0x80 <= octet)
            throw new FormatException($"invalid octet: 0x{octet:X2}");

          decoded[b] = fromBase64Table[octet];

          if (decoded[b] == -1) {
            if (Char.IsWhiteSpace((char)octet)) {
              if (ignoreWhiteSpaces)
                continue;
              else
                throw new FormatException($"invalid octet: 0x{octet:X2}");
            }
            else {
              throw new FormatException($"invalid octet: 0x{octet:X2}");
            }
          }

          b++;
        }

        if (0x40 == decoded[0] || 0x40 == decoded[1])
          throw new FormatException("incorrect padding");

        if (outputBuffer.Length <= outputOffset)
          throw new FormatException("attempt to access beyond end of array"); // NET46 spec

        outputBuffer[outputOffset++] = (byte)((decoded[0] << 2) | (decoded[1] >> 4));
        ret++;

        if (0x40 == decoded[2]) {
          padded = true;
          continue;
        }

        if (outputBuffer.Length <= outputOffset)
          throw new FormatException("attempt to access beyond end of array"); // NET46 spec

        outputBuffer[outputOffset++] = (byte)((decoded[1] << 4) | (decoded[2] >> 2));
        ret++;

        if (0x40 == decoded[3]) {
          padded = true;
          continue;
        }

        if (outputBuffer.Length <= outputOffset)
          throw new FormatException("attempt to access beyond end of array"); // NET46 spec

        outputBuffer[outputOffset++] = (byte)((decoded[2] << 6) | decoded[3]);
        ret++;
      }

      return ret;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);

      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputOffset", inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);

      var ret = new byte[(inputCount / 4 + 1) * 3];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, ret, 0);

      Array.Resize(ref ret, len);

      return ret;
    }

    private static readonly int[] fromBase64Table = new int[] {
    /*0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,*/
        -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1, // 0x00
        -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1, // 0x10
        -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1, 0x3e,   -1,   -1,   -1, 0x3f, // 0x20
        -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1, 0x40,   -1,   -1, // 0x30
        -1, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, // 0x40
      0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,   -1,   -1,   -1,   -1,   -1, // 0x50
        -1, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, // 0x60
      0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33,   -1,   -1,   -1,   -1,   -1, // 0x70
    };

    private bool disposed = false;
    private readonly bool ignoreWhiteSpaces;
  }
}
