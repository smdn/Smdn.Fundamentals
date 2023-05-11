// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;
using System.Text;

using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Security.Cryptography;

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
  private static readonly string mimeEncodingFoldingString = "\r\n\t";
  private static readonly byte[] mimeEncodingPostamble = new byte[] { 0x3f, 0x3d }; // "?="

  public static string Encode(
    string str,
    MimeEncodingMethod encoding
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: Encoding.ASCII,
      doFold: false,
      foldingLimit: default,
      foldingOffset: default,
      foldingString: null!
    );

  public static string Encode(
    string str,
    MimeEncodingMethod encoding,
    Encoding charset
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: charset,
      doFold: false,
      foldingLimit: default,
      foldingOffset: default,
      foldingString: null!
    );

  public static string Encode(
    string str,
    MimeEncodingMethod encoding,
    int foldingLimit,
    int foldingOffset
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: Encoding.ASCII,
      doFold: true,
      foldingLimit: foldingLimit,
      foldingOffset: foldingOffset,
      foldingString: mimeEncodingFoldingString
    );

  public static string Encode(
    string str,
    MimeEncodingMethod encoding,
    int foldingLimit,
    int foldingOffset,
    string foldingString
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: Encoding.ASCII,
      doFold: true,
      foldingLimit: foldingLimit,
      foldingOffset: foldingOffset,
      foldingString: foldingString
    );

  public static string Encode(
    string str,
    MimeEncodingMethod encoding,
    Encoding charset,
    int foldingLimit,
    int foldingOffset
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: charset,
      doFold: true,
      foldingLimit: foldingLimit,
      foldingOffset: foldingOffset,
      foldingString: mimeEncodingFoldingString
    );

  public static string Encode(
    string str,
    MimeEncodingMethod encoding,
    Encoding charset,
    int foldingLimit,
    int foldingOffset,
    string foldingString
  )
    => EncodeCore(
      str: str,
      encoding: encoding,
      charset: charset,
      doFold: true,
      foldingLimit: foldingLimit,
      foldingOffset: foldingOffset,
      foldingString: foldingString
    );

  private static string EncodeCore(
    string str,
    MimeEncodingMethod encoding,
    Encoding charset,
    bool doFold,
    int foldingLimit,
    int foldingOffset,
    string foldingString
  )
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

#pragma warning disable CA2000
    (char encodingChar, ICryptoTransform tsform) = encoding switch {
      MimeEncodingMethod.Base64 => ('b', Base64.CreateToBase64Transform()),
      MimeEncodingMethod.QuotedPrintable => ('q', new ToQuotedPrintableTransform(ToQuotedPrintableTransformMode.MimeEncoding)),
      _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(encoding), encoding),
    };
#pragma warning restore CA2000

    using var transform = tsform;

    var preambleText = string.Concat(
      "=?",
#if SYSTEM_TEXT_ENCODING_BODYNAME
      charset.BodyName,
#else
      charset.WebName,
#endif
      "?",
      encodingChar,
      "?"
    );

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
      byte[]? transformed = null;

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
}
