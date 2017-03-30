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

using Smdn.Text;

namespace Smdn.Formats.QuotedPrintableEncodings {
  public sealed class ToQuotedPrintableTransform : ICryptoTransform {
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

    public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode)
    {
      switch (mode) {
        case ToQuotedPrintableTransformMode.MimeEncoding:
          quoteWhitespaces = true;
          break;
        case ToQuotedPrintableTransformMode.ContentTransferEncoding:
          quoteWhitespaces = false;
          break;
        default:
          throw ExceptionUtils.CreateNotSupportedEnumValue(mode);
      }
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

      var upperCaseHexOctets = Ascii.Octets.GetUpperCaseHexOctets();
      var ret = 0;

      for (var i = 0; i < inputCount; i++) {
        var octet = inputBuffer[inputOffset++];
        var quote = false;

        switch (octet) {
          case Ascii.Octets.HT:
          case Ascii.Octets.SP:
          case 0x3f: // '?'
          case 0x5f: // '_'
            quote = quoteWhitespaces;
            break;

          case 0x3d: // '='
            quote = true;
            break;

          default:
            // quote non-printable chars
            quote = (octet < 0x21 || 0x7f < octet);
            break;
        }

        if (quote) {
          // '=' 0x3d or non printable char
          outputBuffer[outputOffset++] = 0x3d; // '=' 0x3d
          outputBuffer[outputOffset++] = upperCaseHexOctets[octet >> 4];
          outputBuffer[outputOffset++] = upperCaseHexOctets[octet & 0xf];

          ret += 3;
        }
        else {
          outputBuffer[outputOffset++] = octet;

          ret += 1;
        }
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
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("inputCount", inputOffset);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("inputOffset", inputBuffer, inputOffset, inputCount);
      if (InputBlockSize < inputCount)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo("InputBlockSize", "inputCount", inputCount);

      var outputBuffer = new byte[inputCount * OutputBlockSize];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

      Array.Resize(ref outputBuffer, len);

      return outputBuffer;
    }

    private readonly bool quoteWhitespaces = true;
    private bool disposed = false;
  }
}
