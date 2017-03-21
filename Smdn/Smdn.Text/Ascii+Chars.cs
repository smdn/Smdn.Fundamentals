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
  public static class Chars {
    public const char NUL     = '\u0000';
    public const char CR      = '\u000d';
    public const char LF      = '\u000a';
    public const char HT      = '\u0009'; // horizontal tab
    public const char SP      = ' ';
    public const char Quote   = '\'';
    public const char DQuote  = '"';
    public const char Comma   = ',';
    public const char LessThan    = '<';
    public const char GreaterThan = '>';
    public const char Ampersand   = '&';
    public const string CRLF  = "\u000d\u000a";

    internal static readonly char[] LowerCaseHexCharArray = new char[] {
      '0', '1', '2', '3',
      '4', '5', '6', '7',
      '8', '9', 'a', 'b',
      'c', 'd', 'e', 'f'
    };

    internal static readonly char[] UpperCaseHexCharArray = new char[] {
      '0', '1', '2', '3',
      '4', '5', '6', '7',
      '8', '9', 'A', 'B',
      'C', 'D', 'E', 'F'
    };

    public static char[] GetLowerCaseHexChars()
    {
      return (char[])LowerCaseHexCharArray.Clone();
    }

    public static char[] GetUpperCaseHexChars()
    {
      return (char[])UpperCaseHexCharArray.Clone();
    }
  }
}
