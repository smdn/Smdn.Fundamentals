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
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class MimeEncoding {
  public static string Encode(string str, MimeEncodingMethod encoding)
    => Encode(str, encoding, Encoding.ASCII, false, 0, 0, null);

  public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset)
    => Encode(str, encoding, charset, false, 0, 0, null);

  public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset)
    => Encode(str, encoding, Encoding.ASCII, true, foldingLimit, foldingOffset, mimeEncodingFoldingString);

  public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString)
    => Encode(str, encoding, Encoding.ASCII, true, foldingLimit, foldingOffset, foldingString);

  public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset)
    => Encode(str, encoding, charset, true, foldingLimit, foldingOffset, mimeEncodingFoldingString);

  public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString)
    => Encode(str, encoding, charset, true, foldingLimit, foldingOffset, foldingString);

  private static readonly string mimeEncodingFoldingString = "\r\n\t";
  private static readonly byte[] mimeEncodingPostamble = new byte[] { 0x3f, 0x3d }; // "?="

  private static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, bool doFold, int foldingLimit, int foldingOffset, string foldingString)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    if (charset == null)
      throw new ArgumentNullException(nameof(charset));
    if (doFold) {
      if (foldingLimit < 1)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(1, nameof(foldingLimit), foldingLimit);
      if (foldingOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(0, nameof(foldingOffset), foldingOffset);
      if (foldingLimit <= foldingOffset)
        throw ExceptionUtils.CreateArgumentMustBeLessThan(nameof(foldingLimit), nameof(foldingOffset), foldingOffset);
      if (foldingString == null)
        throw new ArgumentNullException(nameof(foldingString));
    }

    (char encodingChar, ICryptoTransform transform) = encoding switch {
      MimeEncodingMethod.Base64 => ('b', Base64.CreateToBase64Transform()),
      MimeEncodingMethod.QuotedPrintable => ('q', new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.MimeEncoding)),
      _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(encoding), encoding),
    };

#if SYSTEM_TEXT_ENCODING_BODYNAME
    var preambleText = string.Concat("=?", charset.BodyName, "?", encodingChar, "?");
#else
    var preambleText = string.Concat("=?", charset.WebName, "?", encodingChar, "?");
#endif

    if (!doFold)
      return string.Concat(preambleText, transform.TransformStringTo(str, charset), "?=");

    // folding
    var ret = new StringBuilder();
    var preamble = Encoding.ASCII.GetBytes(preambleText);
    var firstLine = true;
    var inputCharBuffer = str.ToCharArray();
    var inputCharOffset = 0;
    var outputBuffer = new byte[foldingLimit];
    var ambleLength = preamble.Length + mimeEncodingPostamble.Length;
    var outputLimit = foldingLimit - (foldingOffset + ambleLength);

    if (outputLimit <= 0)
      throw new ArgumentOutOfRangeException(nameof(foldingLimit), foldingLimit, "too short");

    // copy preamble to buffer
    Buffer.BlockCopy(preamble, 0, outputBuffer, 0, preamble.Length);

    for (; ; ) {
      var inputBlockSizeLimit = (outputLimit * transform.InputBlockSize / transform.OutputBlockSize) - 1;
      var transformCharCount = 0;
      var outputCount = preamble.Length;

      // decide char count to transform
      for (transformCharCount = inputBlockSizeLimit / charset.GetMaxByteCount(1); ; transformCharCount++) {
        if (inputCharBuffer.Length <= inputCharOffset + transformCharCount) {
          transformCharCount = inputCharBuffer.Length - inputCharOffset;
          break;
        }

        if (inputBlockSizeLimit <= charset.GetByteCount(inputCharBuffer, inputCharOffset, transformCharCount + 1))
          break;
      }

      // transform chars
      byte[] transformed = null;

      for (; ; ) {
        var bytes = charset.GetBytes(inputCharBuffer, inputCharOffset, transformCharCount);
        var t = transform.TransformBytes(bytes, 0, bytes.Length);

        if (transformed == null || t.Length <= outputLimit) {
          transformed = t;

          if (inputCharBuffer.Length <= inputCharOffset + transformCharCount + 1)
            break;

          transformCharCount++;
          continue;
        }
        else {
          transformCharCount--;
          break;
        }
      }

      if (outputBuffer.Length < ambleLength + transformed.Length)
        throw new ArgumentOutOfRangeException(nameof(foldingLimit), foldingLimit, $"too short, at least {ambleLength + transformed.Length} is required");

      // copy transformed chars to buffer
      Buffer.BlockCopy(transformed, 0, outputBuffer, outputCount, transformed.Length);

      outputCount += transformed.Length;

      // copy postanble to buffer
      Buffer.BlockCopy(mimeEncodingPostamble, 0, outputBuffer, outputCount, mimeEncodingPostamble.Length);

      outputCount += mimeEncodingPostamble.Length;

      ret.Append(Encoding.ASCII.GetString(outputBuffer, 0, outputCount));

      inputCharOffset += transformCharCount;

      if (inputCharOffset < inputCharBuffer.Length) {
        ret.Append(foldingString);

        if (firstLine) {
          outputLimit = foldingLimit - ambleLength;
          firstLine = false;
        }
      }
      else {
        break;
      }
    }

    return ret.ToString();
  }

  public static string Decode(string str)
    => Decode(str, null, null, out _, out _);

  public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding)
    => Decode(str, selectFallbackEncoding, null, out _, out _);

  public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported)
    => Decode(str, selectFallbackEncoding, decodeMalformedOrUnsupported, out _, out _);

  public static string Decode(string str, out MimeEncodingMethod encoding, out Encoding charset)
    => Decode(str, null, null, out encoding, out charset);

  public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset)
    => Decode(str, selectFallbackEncoding, null, out encoding, out charset);

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
  private static readonly Regex mimeEncodedWordRegex = new(
    @"\s*=\?(?<charset>[^?*]+)(?<language>\*[^?]+)?\?(?<encoding>[^?]+)\?(?<text>[^\?\s]+)\?=\s*",
    RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled
  );

  public static string Decode(
    string str,
    EncodingSelectionCallback selectFallbackEncoding,
    MimeEncodedWordConverter decodeMalformedOrUnsupported,
    out MimeEncodingMethod encoding,
    out Encoding charset
  )
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));

    charset = null;
    encoding = MimeEncodingMethod.None;

    Encoding lastCharset = null;
    var lastEncoding = MimeEncodingMethod.None;

    var ret = mimeEncodedWordRegex.Replace(str, m => {
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
    });

    charset = lastCharset;
    encoding = lastEncoding;

    return ret;
  }

  public static string DecodeNullable(string str)
  {
    if (str == null)
      return null;

    return Decode(str, null, null, out _, out _);
  }

  public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding)
  {
    if (str == null)
      return null;

    return Decode(str, selectFallbackEncoding, null, out _, out _);
  }

  public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported)
  {
    if (str == null)
      return null;

    return Decode(str, selectFallbackEncoding, decodeMalformedOrUnsupported, out _, out _);
  }

  public static string DecodeNullable(string str, out MimeEncodingMethod encoding, out Encoding charset)
  {
    if (str == null) {
      encoding = MimeEncodingMethod.None;
      charset = null;

      return null;
    }
    else {
      return Decode(str, null, null, out encoding, out charset);
    }
  }

  public static string DecodeNullable(
    string str,
    EncodingSelectionCallback selectFallbackEncoding,
    out MimeEncodingMethod encoding,
    out Encoding charset
  )
  {
    if (str == null) {
      encoding = MimeEncodingMethod.None;
      charset = null;

      return null;
    }
    else {
      return Decode(str, selectFallbackEncoding, null, out encoding, out charset);
    }
  }

  public static string DecodeNullable(
    string str,
    EncodingSelectionCallback selectFallbackEncoding,
    MimeEncodedWordConverter decodeMalformedOrUnsupported,
    out MimeEncodingMethod encoding,
    out Encoding charset
  )
  {
    if (str == null) {
      encoding = MimeEncodingMethod.None;
      charset = null;

      return null;
    }
    else {
      return Decode(str, selectFallbackEncoding, decodeMalformedOrUnsupported, out encoding, out charset);
    }
  }
}
