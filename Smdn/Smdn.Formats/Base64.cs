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
using System.Text;

namespace Smdn.Formats {
  public static class Base64 {
    public static string Encode(string str)
    {
      return Encode(str, Encoding.ASCII);
    }

    public static string Encode(string str, Encoding encoding)
    {
      return Encode(encoding.GetBytes(str));
    }

    public static string Encode(byte[] bytes)
    {
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
    }

    public static string Decode(string str)
    {
      return Decode(str, Encoding.ASCII);
    }

    public static string Decode(string str, Encoding encoding)
    {
      return encoding.GetString(DecodeToByteArray(str));
    }

    public static byte[] DecodeToByteArray(string str)
    {
      return System.Convert.FromBase64String(str);
    }
  }
}
