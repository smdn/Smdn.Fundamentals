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
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  [Obsolete("use Smdn.Formats.PercentEncodings.PercentEncoding instead")]
  public static class PercentEncoding {
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetEncodedString(str, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode);
    }

    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetEncodedString(str, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode, encoding);
    }

    public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetEncodedString(bytes, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode);
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetEncodedString(bytes, offset, count, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode);
    }

    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.Encode(str, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode);
    }

    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.Encode(str, (Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode)mode, encoding);
    }

    public static string GetDecodedString(string str)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetDecodedString(str);
    }

    public static string GetDecodedString(string str, bool decodePlusToSpace)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetDecodedString(str, decodePlusToSpace);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetDecodedString(str, encoding);
    }

    public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.GetDecodedString(str, encoding, decodePlusToSpace);
    }

    public static byte[] Decode(string str)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.Decode(str);
    }

    public static byte[] Decode(string str, bool decodePlusToSpace)
    {
      return Smdn.Formats.PercentEncodings.PercentEncoding.Decode(str, decodePlusToSpace);
    }
  }
}
