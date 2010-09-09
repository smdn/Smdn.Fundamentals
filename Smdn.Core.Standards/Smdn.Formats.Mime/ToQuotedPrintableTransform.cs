// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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

namespace Smdn.Formats.Mime {
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

    public ToQuotedPrintableTransform()
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

        if ((0x21 <= octet && octet <= 0x3c) ||
            (0x3e <= octet && octet <= 0x7e) ||
            octet == Octets.HT ||
            octet == Octets.SP) {
          // printable char (except '=' 0x3d)
          outputBuffer[outputOffset++] = octet;

          ret += 1;
        }
        else {
          // '=' 0x3d or non printable char
          outputBuffer[outputOffset++] = 0x3d; // '=' 0x3d
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
        throw new ArgumentOutOfRangeException("inputCount", inputCount, "input length too long");

      var outputBuffer = new byte[inputCount * OutputBlockSize];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputBuffer.Length);

      Array.Resize(ref outputBuffer, len);

      return outputBuffer;
    }

    private bool disposed = false;
  }
}
