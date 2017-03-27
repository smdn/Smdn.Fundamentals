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

namespace Smdn.Formats {
  [Obsolete("use Smdn.Text.Ascii.Hexadecimals instead")]
  public static class Hexadecimals {
    public static string ToLowerString(byte[] bytes)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToLowerString(bytes);
    }

    public static string ToUpperString(byte[] bytes)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToUpperString(bytes);
    }

    public static byte[] ToLowerByteArray(byte[] bytes)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToLowerByteArray(bytes);
    }

    public static byte[] ToUpperByteArray(byte[] bytes)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToUpperByteArray(bytes);
    }

    public static byte[] ToByteArray(string hexString)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToByteArray(hexString);
    }

    public static byte[] ToByteArrayFromLowerString(string lowerCasedString)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToByteArrayFromLowerString(lowerCasedString);
    }

    public static byte[] ToByteArrayFromUpperString(string upperCasedString)
    {
      return Smdn.Text.Ascii.Hexadecimals.ToByteArrayFromUpperString(upperCasedString);
    }
  }
}
