// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
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

namespace Smdn.Formats {
  /*
   * http://tools.ietf.org/html/rfc3986
   * RFC 3986 - Uniform Resource Identifier (URI): Generic Syntax
   * 2.1. Percent-Encoding
   * 
   * http://tools.ietf.org/html/rfc2396
   * RFC 2396 - Uniform Resource Identifiers (URI): Generic Syntax
   */
  public class ToPercentEncodedTransform : ICryptoTransform {
    //                                    "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
    // RFC 2396 unreserved characters:    "!      '()*  -. 0123456789     ?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[  ] _ abcdefghijklmnopqrstuvwxyz   ~";
    // RFC 3986 unreserved characters:    "             -. 0123456789       ABCDEFGHIJKLMNOPQRSTUVWXYZ     _ abcdefghijklmnopqrstuvwxyz   ~";
    private const string
      rfc2396UriEscapeChars             = " \"  %                      < >                            [\\]^ `                          {|} ";
    private const string
      rfc3986UriEscapeChars             = "!\"  % '()*                 < >?@                          [\\]^ `                          {|} ";
    private const string
      rfc2396DataEscapeChars            = " \"#$%&    +,  /          :;<=>?@                          [\\]^ `                          {|} ";
    private const string
      rfc3986DataEscapeChars            = "!\"#$%&'()*+,  /          :;<=>?@                          [\\]^ `                          {|} ";

    private const string
      rfc5092AChars                     = " \"# %         /          :;< >?@                          [\\]^ `                          {|} ";
    private const string
      rfc5092BChars                     = " \"# %                     ;< >?                           [\\]^ `                          {|} ";

    private byte[] GetEscapeOctets(string str)
    {
      var chars = str.ToCharArray();
      var octets = new byte[0x80 - 0x20];
      var count = 0;

      octets[count++] = Octets.SP;

      for (var i = 0; i < chars.Length; i++) {
        if (chars[i] != Chars.SP)
          octets[count++] = (byte)chars[i];
      }

      Array.Resize(ref octets, count);

      return octets;
    }

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

    public ToPercentEncodedTransform(ToPercentEncodedTransformMode mode)
    {
      switch (mode & ToPercentEncodedTransformMode.ModeMask) {
        case ToPercentEncodedTransformMode.Rfc2396Uri:
          escapeOctets = GetEscapeOctets(rfc2396UriEscapeChars);
          break;
        case ToPercentEncodedTransformMode.Rfc2396Data:
          escapeOctets = GetEscapeOctets(rfc2396DataEscapeChars);
          break;
        case ToPercentEncodedTransformMode.Rfc3986Uri:
          escapeOctets = GetEscapeOctets(rfc3986UriEscapeChars);
          break;
        case ToPercentEncodedTransformMode.Rfc3986Data:
          escapeOctets = GetEscapeOctets(rfc3986DataEscapeChars);
          break;
        case ToPercentEncodedTransformMode.Rfc5092Uri:
          escapeOctets = GetEscapeOctets(rfc5092AChars);
          break;
        case ToPercentEncodedTransformMode.Rfc5092Path:
          escapeOctets = GetEscapeOctets(rfc5092BChars);
          break;
        default:
          throw new NotSupportedException(string.Format("unsupported transform mode: {0}", mode));
      }

      escapeSpaceToPlus = (int)(mode & ToPercentEncodedTransformMode.EscapeSpaceToPlus) != 0;
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

        var escape =
          !((0x30 <= octet && octet <= 0x39) || // DIGIT
            (0x41 <= octet && octet <= 0x5a) || // UPALPHA
            (0x61 <= octet && octet <= 0x7a) // LOWALPHA
           ) &&
          (octet < 0x20 || 0x80 <= octet);

        escape |= (0 <= Array.BinarySearch(escapeOctets, octet));

        if (escape) {
          if (octet == 0x20 && escapeSpaceToPlus) {
            outputBuffer[outputOffset++] = 0x2b; // '+' 0x2b

            ret += 1;
          }
          else {
            outputBuffer[outputOffset++] = 0x25; // '%' 0x25
            outputBuffer[outputOffset++] = Octets.UpperCaseHexOctets[octet >> 4];
            outputBuffer[outputOffset++] = Octets.UpperCaseHexOctets[octet & 0xf];

            ret += 3;
          }
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
    private byte[] escapeOctets;
    private bool escapeSpaceToPlus;
  }
}
