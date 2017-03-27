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
  [Obsolete("use Smdn.Text.Ascii.Octets instead")]
  public static class Octets {
    public const byte NUL   = 0x00;
    public const byte CR    = 0x0d;
    public const byte LF    = 0x0a;
    public const byte SP    = 0x20;
    public const byte HT    = 0x09; // horizontal tab

    public static byte[] GetCRLF()
    {
      return Smdn.Text.Ascii.Octets.GetCRLF();
    }

    public static byte[] GetLowerCaseHexOctets()
    {
      return Smdn.Text.Ascii.Octets.GetLowerCaseHexOctets();
    }

    public static byte[] GetUpperCaseHexOctets()
    {
      return Smdn.Text.Ascii.Octets.GetUpperCaseHexOctets();
    }

    public static byte[] GetToLowerCaseAsciiTable()
    {
      return Smdn.Text.Ascii.Octets.GetToLowerCaseAsciiTable();
    }

    public static byte[] GetToUpperCaseAsciiTable()
    {
      return Smdn.Text.Ascii.Octets.GetToUpperCaseAsciiTable();
    }

    public static bool IsDecimalNumber(byte b)
    {
      return Smdn.Text.Ascii.Octets.IsDecimalNumber(b);
    }
  }
}
