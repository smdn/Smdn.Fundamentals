// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Text;

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

    [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexChars instead")]
    public static IReadOnlyList<char> LowerCaseHexChars => Smdn.Formats.Hexadecimal.LowerCaseHexChars.ToArray();

    [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexChars instead")]
    public static IReadOnlyList<char> UpperCaseHexChars => Smdn.Formats.Hexadecimal.UpperCaseHexChars.ToArray();

    [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexChars instead")]
    public static char[] GetLowerCaseHexChars() => Smdn.Formats.Hexadecimal.LowerCaseHexChars.ToArray();

    [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexChars instead")]
    public static char[] GetUpperCaseHexChars() => Smdn.Formats.Hexadecimal.UpperCaseHexChars.ToArray();
  }
}
