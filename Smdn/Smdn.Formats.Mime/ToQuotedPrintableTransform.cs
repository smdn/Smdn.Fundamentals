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

namespace Smdn.Formats.Mime {
  [Obsolete("use Smdn.Formats.QuotedPrintable.ToQuotedPrintableTransform instead")]
  public sealed class ToQuotedPrintableTransform : ICryptoTransform {
    public bool CanTransformMultipleBlocks {
      get { return inst.CanTransformMultipleBlocks; }
    }

    public bool CanReuseTransform {
      get { return inst.CanReuseTransform; }
    }

    public int InputBlockSize {
      get { return inst.InputBlockSize; }
    }

    public int OutputBlockSize {
      get { return inst.OutputBlockSize; }
    }

    public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode)
    {
      inst = new QuotedPrintable.ToQuotedPrintableTransform((QuotedPrintable.ToQuotedPrintableTransformMode)mode);
    }

    public void Clear()
    {
      inst.Clear();
    }

    void IDisposable.Dispose()
    {
      (inst as IDisposable).Dispose();
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      return inst.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      return inst.TransformFinalBlock(inputBuffer, inputOffset, inputCount);
    }

    private readonly Smdn.Formats.QuotedPrintable.ToQuotedPrintableTransform inst;
  }
}
