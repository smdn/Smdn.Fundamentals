// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Security.Cryptography;
using Smdn.Text.Encodings;

namespace Smdn.Formats.Mime;

/*
 * http://tools.ietf.org/html/rfc2047
 * RFC 2047 - MIME (Multipurpose Internet Mail Extensions) Part Three: Message Header Extensions for Non-ASCII Text
 * 2. Syntax of encoded-words
 * 3. Character sets
 * 4. Encodings

 * encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
 * charset = token    ; see section 3
 * encoding = token   ; see section 4
 * token = 1*<Any CHAR except SPACE, CTLs, and especials>
 * especials = "(" / ")" / "<" / ">" / "@" / "," / ";" / ":" / "
 *             <"> / "/" / "[" / "]" / "?" / "." / "="
 * encoded-text = 1*<Any printable ASCII character other than "?"
 *                   or SPACE>
 *               ; (but see "Use of encoded-words in message
 *               ; headers", section 5)
 */
#pragma warning disable IDE0040
static partial class MimeEncoding {
#pragma warning restore IDE0040
  public static string Decode(string str)
    => Decode(
      str: str,
      selectFallbackEncoding: null,
      decodeMalformedOrUnsupported: null,
      encoding: out _,
      charset: out _
    );

  public static string Decode(
    string str,
    EncodingSelectionCallback? selectFallbackEncoding
  )
    => Decode(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: null,
      encoding: out _,
      charset: out _
    );

  public static string Decode(
    string str,
    EncodingSelectionCallback? selectFallbackEncoding,
    MimeEncodedWordConverter? decodeMalformedOrUnsupported
  )
    => Decode(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: decodeMalformedOrUnsupported,
      encoding: out _,
      charset: out _
    );

  public static string Decode(
    string str,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
    => Decode(
      str: str,
      selectFallbackEncoding: null,
      decodeMalformedOrUnsupported: null,
      encoding: out encoding,
      charset: out charset
    );

  public static string Decode(
    string str,
    EncodingSelectionCallback? selectFallbackEncoding,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
    => Decode(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: null,
      encoding: out encoding,
      charset: out charset
    );

  /*
   * RFC 2231 - MIME Parameter Value and Encoded Word Extensions: Character Sets, Languages, and Continuations
   * http://tools.ietf.org/html/rfc2231
   * http://www.rfc-editor.org/errata_search.php?rfc=2231
   *
   * The ABNF given in RFC 2047 for encoded-words is:
   *
   *    encoded-word := "=?" charset "?" encoding "?" encoded-text "?="
   *
   * This specification changes this ABNF to:
   *
   *       encoded-word := "=?" charset ["*" language] "?" encoding "?"
   *                       encoded-text "?="
   */
#pragma warning disable SA1203
  private const string MimeEncodedWordPattern
    = @"\s*=\?(?<charset>[^?*]+)(?<language>\*[^?]+)?\?(?<encoding>[^?]+)\?(?<text>[^\?\s]+)\?=\s*";
#pragma warning restore SA1203

  private static readonly Regex mimeEncodedWordRegex = new(
    MimeEncodedWordPattern,
    RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled
  );

#if SYSTEM_TEXT_REGULAREXPRESSIONS_REGEXGENERATORATTRIBUTE
  [RegexGenerator(MimeEncodedWordPattern, RegexOptions.Singleline | RegexOptions.CultureInvariant)]
  private static partial Regex GetMimeEncodedWordRegex();
#endif

  public static string Decode(
    string str,
    EncodingSelectionCallback? selectFallbackEncoding,
    MimeEncodedWordConverter? decodeMalformedOrUnsupported,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));

    charset = null;
    encoding = MimeEncodingMethod.None;

    Encoding? lastCharset = null;
    var lastEncoding = MimeEncodingMethod.None;

    var ret =
#if SYSTEM_TEXT_REGULAREXPRESSIONS_REGEXGENERATORATTRIBUTE
      GetMimeEncodedWordRegex()
#else
      mimeEncodedWordRegex
#endif
      .Replace(
        str,
        m => {
          // charset
          var charsetString = m.Groups["charset"].Value;

          lastCharset = EncodingUtils.GetEncoding(charsetString, selectFallbackEncoding);

          if (lastCharset == null)
            throw new EncodingNotSupportedException(charsetString, $"'{charsetString}' is an unsupported or invalid charset");

          // encoding
          var encodingString = m.Groups["encoding"].Value;
          var encodedText = m.Groups["text"].Value;
          ICryptoTransform transform;

          switch (encodingString) {
            case "b":
            case "B":
              lastEncoding = MimeEncodingMethod.Base64;
              transform = Base64.CreateFromBase64Transform(ignoreWhiteSpaces: true);
              break;
            case "q":
            case "Q":
              lastEncoding = MimeEncodingMethod.QuotedPrintable;
              transform = new FromQuotedPrintableTransform(FromQuotedPrintableTransformMode.MimeEncoding);
              break;
            default:
              if (decodeMalformedOrUnsupported == null)
                throw new FormatException($"{encodingString} is an invalid encoding");
              else
                return decodeMalformedOrUnsupported(lastCharset, encodingString, encodedText) ?? m.Value;
          }

          try {
            return transform.TransformStringFrom(encodedText, lastCharset);
          }
          catch (FormatException) {
            if (decodeMalformedOrUnsupported == null)
              throw;
            else
              return decodeMalformedOrUnsupported(lastCharset, encodingString, encodedText) ?? m.Value;
          }
        }
      );

    charset = lastCharset;
    encoding = lastEncoding;

    return ret;
  }

  public static string? DecodeNullable(string? str)
    => DecodeNullable(
      str: str,
      selectFallbackEncoding: null,
      decodeMalformedOrUnsupported: null,
      encoding: out _,
      charset: out _
    );

  public static string? DecodeNullable(
    string? str,
    EncodingSelectionCallback? selectFallbackEncoding
  )
    => DecodeNullable(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: null,
      encoding: out _,
      charset: out _
    );

  public static string? DecodeNullable(
    string? str,
    EncodingSelectionCallback? selectFallbackEncoding,
    MimeEncodedWordConverter? decodeMalformedOrUnsupported
  )
    => DecodeNullable(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: decodeMalformedOrUnsupported,
      encoding: out _,
      charset: out _
    );

  public static string? DecodeNullable(
    string? str,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
    => DecodeNullable(
      str: str,
      selectFallbackEncoding: null,
      decodeMalformedOrUnsupported: null,
      encoding: out encoding,
      charset: out charset
    );

  public static string? DecodeNullable(
    string? str,
    EncodingSelectionCallback? selectFallbackEncoding,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
    => DecodeNullable(
      str: str,
      selectFallbackEncoding: selectFallbackEncoding,
      decodeMalformedOrUnsupported: null,
      encoding: out encoding,
      charset: out charset
    );

  public static string? DecodeNullable(
    string? str,
    EncodingSelectionCallback? selectFallbackEncoding,
    MimeEncodedWordConverter? decodeMalformedOrUnsupported,
    out MimeEncodingMethod encoding,
    out Encoding? charset
  )
  {
    if (str == null) {
      encoding = MimeEncodingMethod.None;
      charset = null;

      return null;
    }

    return Decode(
      str,
      selectFallbackEncoding,
      decodeMalformedOrUnsupported,
      out encoding,
      out charset
    );
  }
}
