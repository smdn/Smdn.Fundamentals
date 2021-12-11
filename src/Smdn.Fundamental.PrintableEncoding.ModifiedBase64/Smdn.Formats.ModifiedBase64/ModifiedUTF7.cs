// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

/*
 * RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
 * 5.1.3. Mailbox International Naming Convention
 * http://tools.ietf.org/html/rfc3501#section-5.1.3
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class ModifiedUTF7 {
  public static string Encode(string str)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    if (str.Length == 0)
      return string.Empty;

    using var transform = new ToRFC3501ModifiedBase64Transform();

    var encoded = new StringBuilder(str.Length * 2);
    var index = -1;
    var shiftFrom = -1;

    foreach (var c in str) {
      index++;

      if (c is >= '\u0020' and <= '\u007e') {
        if (0 <= shiftFrom) {
          // string -> modified UTF7
          encoded.Append('&');
          encoded.Append(transform.TransformStringTo(str.Substring(shiftFrom, index - shiftFrom), Encoding.BigEndianUnicode));
          encoded.Append('-');

          shiftFrom = -1;
        }

        // printable US-ASCII characters
        if (c == '\u0026')
          // except for "&"
          encoded.Append("&-");
        else
          encoded.Append(c);
      }
      else {
        if (shiftFrom == -1)
          shiftFrom = index;
      }
    }

    if (0 <= shiftFrom) {
      // string -> modified UTF7
      encoded.Append('&');
      encoded.Append(transform.TransformStringTo(str.Substring(shiftFrom), Encoding.BigEndianUnicode));
      encoded.Append('-');
    }

    return encoded.ToString();
  }

  public static string Decode(string str)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    if (str.Length == 0)
      return string.Empty;

    using var transform = new FromRFC3501ModifiedBase64Transform(ignoreWhiteSpaces: false);

    var decoded = new StringBuilder(str.Length);

    for (var index = 0; index < str.Length; index++) {
      var c = str[index];

      if (c is < '\u0020' or > '\u007e')
        throw new FormatException($"contains non-ascii or non-printable character: at index {index} of '{str}', \\u{(int)c:x4}");

      // In modified UTF-7, printable US-ASCII characters, except for "&",
      // represent themselves
      // "&" is used to shift to modified BASE64
      if (c != '&') {
        decoded.Append(c);
        continue;
      }

      if (str.Length <= ++index)
        // incorrect form
        throw new FormatException("incorrect form");

      if (str[index] == '-') {
        // The character "&" (0x26) is represented by the two-octet sequence "&-".
        decoded.Append('&');
        continue;
      }

      var nonPrintableChars = new byte[str.Length - index]; // TODO: array pool
      var len = 0;

      for (; index < str.Length; index++) {
        c = str[index];

        if (c == '-')
          // "-" is used to shift back to US-ASCII
          break;
        else
          nonPrintableChars[len++] = (byte)c;
      }

      // modified UTF7 -> string
      decoded.Append(Encoding.BigEndianUnicode.GetString(transform.TransformBytes(nonPrintableChars, 0, len)));
    }

    return decoded.ToString();
  }
}
