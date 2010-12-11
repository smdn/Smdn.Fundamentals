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

namespace Smdn.Formats {
  public static class Octets {
    public const byte NUL   = 0x00;
    public const byte CR    = 0x0d;
    public const byte LF    = 0x0a;
    public const byte SP    = 0x20;
    public const byte HT    = 0x09; // horizontal tab

    internal static readonly byte[] CRLFArray = new byte[] {0x0d, 0x0a};

    [Obsolete("use GetCRLF() instead", true)]
    public static byte[] CRLF {
      get { throw new NotImplementedException(); }
    }

    public static byte[] GetCRLF()
    {
      return (byte[])CRLFArray.Clone();
    }

    internal static readonly byte[] LowerCaseHexOctetArray = new byte[] {
      0x30, 0x31, 0x32, 0x33,
      0x34, 0x35, 0x36, 0x37,
      0x38, 0x39, 0x61, 0x62,
      0x63, 0x64, 0x65, 0x66,
    };

    internal static readonly byte[] UpperCaseHexOctetArray = new byte[] {
      0x30, 0x31, 0x32, 0x33,
      0x34, 0x35, 0x36, 0x37,
      0x38, 0x39, 0x41, 0x42,
      0x43, 0x44, 0x45, 0x46,
    };

    [Obsolete("use GetLowerCaseHexOctetArray() instead", true)]
    public static byte[] LowerCaseHexOctets {
      get { throw new NotImplementedException(); }
    }

    [Obsolete("use GetUpperCaseHexOctetArray() instead", true)]
    public static byte[] UpperCaseHexOctets {
      get { throw new NotImplementedException(); }
    }

    public static byte[] GetLowerCaseHexOctets()
    {
      return (byte[])LowerCaseHexOctetArray.Clone();
    }

    public static byte[] GetUpperCaseHexOctets()
    {
      return (byte[])UpperCaseHexOctetArray.Clone();
    }

    public static bool IsDecimalNumber(byte b)
    {
      return (0x30 <= b && b <= 0x39);
    }
  }
}
