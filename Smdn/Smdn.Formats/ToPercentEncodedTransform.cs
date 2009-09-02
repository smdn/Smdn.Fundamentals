// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
  /*
   * http://tools.ietf.org/html/rfc3986
   * RFC 3986 - Uniform Resource Identifier (URI): Generic Syntax
   * 2.1. Percent-Encoding
   */
  public class ToPercentEncodedTransform : ICryptoTransform {
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

    public ToPercentEncodedTransform()
    {
    }

    ~ToPercentEncodedTransform()
    {
      Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
      disposed = true;
    }

    public void Clear()
    {
      Dispose(true);
    }

    void IDisposable.Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
      if (inputBuffer == null)
        throw new ArgumentNullException("inputBuffer");
      if (inputOffset < 0)
        throw new ArgumentException("inputOffset < 0", "inputOffset");
      if (inputBuffer.Length < inputCount)
        throw new ArgumentException("inputBuffer.Length < inputCount", "inputCount");
      if (inputBuffer.Length - inputCount < inputOffset)
        throw new ArgumentException("inputBuffer.Length - inputCount < inputOffset", "inputOffset");
      if (outputBuffer == null)
        throw new ArgumentNullException("outputBuffer");
      if (outputOffset < 0)
        throw new ArgumentException("outputOffset < 0", "outputOffset");
      if (outputBuffer.Length < inputCount)
        throw new ArgumentException("outputBuffer.Length < inputCount", "outputBuffer");
      if (outputBuffer.Length - inputCount < outputOffset)
        throw new ArgumentException("outputBuffer.Length - inputCount < outputOffset", "outputOffset");

      var ret = 0;

      for (var i = 0; i < inputCount; i++) {
        var octet = inputBuffer[inputOffset++];

        // unreserved    = ALPHA / DIGIT / "-" / "." / "_" / "~"
        if ((0x30 <= octet && octet <= 0x39) || // DIGIT
            (0x41 <= octet && octet <= 0x5a) || (0x61 <= octet && octet <= 0x7a) || // ALPHA
            octet == 0x2d || // '-'
            octet == 0x2e || // '.'
            octet == 0x5f || // '_'
            octet == 0x7e) { // '~'
          outputBuffer[outputOffset++] = octet;

          ret += 1;
        }
        else {
          outputBuffer[outputOffset++] = 0x25; // '%' 0x25
          outputBuffer[outputOffset++] = Octets.UpperCaseHexOctets[octet >> 4];
          outputBuffer[outputOffset++] = Octets.UpperCaseHexOctets[octet & 0xf];

          ret += 3;
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
      if (inputCount < 0)
        throw new ArgumentException("inputCount < 0", "inputCount");
      if (inputBuffer.Length < inputCount)
        throw new ArgumentException("inputBuffer.Length < inputCount", "inputCount");
      if (inputBuffer.Length - inputCount < inputOffset)
        throw new ArgumentException("inputBuffer.Length - inputCount < inputOffset", "inputOffset");
      if (InputBlockSize < inputCount)
        throw new ArgumentOutOfRangeException("inputCount", "input length too long");

      var outputBuffer = new byte[inputCount * OutputBlockSize];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

      Array.Resize(ref outputBuffer, len);

      return outputBuffer;
    }

    private bool disposed = false;
  }
}
