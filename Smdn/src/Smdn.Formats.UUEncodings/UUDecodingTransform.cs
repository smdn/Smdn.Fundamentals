//
// Author:
//       smdn <smdn@smdn.jp>
//
// Copyright (c) 2010-2017 smdn
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

using Smdn.Text;

namespace Smdn.Formats.UUEncodings {
  public sealed class UUDecodingTransform : ICryptoTransform {
    public bool CanTransformMultipleBlocks {
      get { return true; }
    }

    public bool CanReuseTransform {
      get { return true; }
    }

    public int InputBlockSize {
      get { return 1; }
    }

    public int OutputBlockSize {
      get { return 3; }
    }

    public UUDecodingTransform()
    {
    }

    public void Clear()
    {
      disposed = true;
    }

    void IDisposable.Dispose()
    {
      Clear();
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
      if (outputBuffer.Length - inputCount < outputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("outputOffset", outputBuffer, outputOffset, inputCount);

      var ret = 0;

      for (;;) {
        if (bufferOffset == 4)
          ret += WriteBlock(outputBuffer, ref outputOffset);

        if (inputCount-- == 0)
          break;

        var octet = inputBuffer[inputOffset++];

        if (octet == Ascii.Octets.CR || octet == Ascii.Octets.LF) {
          /*
           * <newline>
           */
          if (0 < lineLength)
            ret += WriteBlock(outputBuffer, ref outputOffset);

          lineLength = -1;

          continue;
        }

        if (lineLength == -1) {
          /*
           * <length character>
           */
          if (0x21 <= octet && octet <= 0x5f)
            // '!' 0x21 to '_' 0x5f
            lineLength = (int)(octet - 0x20);
          else if (octet == 0x20 || octet == 0x60)
            // SP 0x20 or '`' 0x60
            lineLength = 0;
          else
            throw new FormatException("incorrect form (line length)");
        }
        else {
          /*
           * <formatted characters>
           */
          if (0x21 <= octet && octet <= 0x5f) {
            // '!' 0x21 to '_' 0x5f
            buffer |= ((long)(octet - 0x20) << (6 * (3 - bufferOffset)));
            bufferOffset++;
          }
          else if (octet == 0x20 || octet == 0x60) {
            // SP 0x20 or '`' 0x60
            bufferOffset++;
            // buffer |= 0x00;
          }
          else {
            throw new FormatException("incorrect form");
          }
        }
      }

      return ret;
    }

    private int WriteBlock(byte[] outputBuffer, ref int outputOffset)
    {
      var ret = 0;

      for (var shift = 16; 0 <= shift; shift -= 8) {
        if (lineLength <= 0)
          break;

        outputBuffer[outputOffset++] = (byte)((buffer >> shift) & 0xff);
        ret++;
        lineLength--;
      }

      buffer = 0L;
      bufferOffset = 0;

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
      if (InputBlockSize < inputCount)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo("InputBlockSize", "inputCount", inputCount);

      var outputBuffer = new byte[inputCount * OutputBlockSize];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);

      if (outputBuffer.Length < len + OutputBlockSize)
        Array.Resize(ref outputBuffer, len + OutputBlockSize); // XXX

      var outputOffset = len;

      len += WriteBlock(outputBuffer, ref outputOffset);

      Array.Resize(ref outputBuffer, len);

      return outputBuffer;
    }

    private long buffer = 0L;
    private int bufferOffset = 0;
    private int lineLength = -1;
    private bool disposed = false;
  }
}