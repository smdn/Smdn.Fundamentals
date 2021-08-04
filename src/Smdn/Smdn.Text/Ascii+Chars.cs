// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Text {
  public static partial class Ascii {
    public static class Chars {
      public const char NUL = '\u0000';
      public const char CR = '\u000d';
      public const char LF = '\u000a';
      public const char HT = '\u0009'; // horizontal tab
      public const char SP = ' ';
      public const char Quote = '\'';
      public const char DQuote = '"';
      public const char Comma = ',';
      public const char LessThan = '<';
      public const char GreaterThan = '>';
      public const char Ampersand = '&';
      public const string CRLF = "\u000d\u000a";

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

      public static IReadOnlyList<char> LowerCaseHexChars => LowerCaseHexCharArray;
      public static IReadOnlyList<char> UpperCaseHexChars => UpperCaseHexCharArray;

      [Obsolete("use LowerCaseHexChars instead")]
      public static char[] GetLowerCaseHexChars()
      {
        return (char[])LowerCaseHexCharArray.Clone();
      }

      [Obsolete("use UpperCaseHexChars instead")]
      public static char[] GetUpperCaseHexChars()
      {
        return (char[])UpperCaseHexCharArray.Clone();
      }
    }
  }
}
