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
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.Mime {
  [Obsolete("use Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding instead")]
  public static class QuotedPrintableEncoding {
    public static string GetEncodedString(string str)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetEncodedString(str);
    }
    
    public static string GetEncodedString(string str, Encoding encoding)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetEncodedString(str, encoding);
    }

    public static string GetEncodedString(byte[] bytes)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetEncodedString(bytes);
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetEncodedString(bytes, offset, count);
    }

    public static byte[] Encode(string str)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.Encode(str);
    }

    public static byte[] Encode(string str, Encoding encoding)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.Encode(str, encoding);
    }

    public static string GetDecodedString(string str)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetDecodedString(str);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.GetDecodedString(str, encoding);
    }

    public static byte[] Decode(string str)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.Decode(str);
    }

    public static Stream CreateEncodingStream(Stream stream)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.CreateEncodingStream(stream);
    }

    public static Stream CreateDecodingStream(Stream stream)
    {
      return Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding.CreateEncodingStream(stream);
    }
  }
}
