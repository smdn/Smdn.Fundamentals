// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET6_0_OR_GREATER
#define SYSTEM_STRING_COPYTO_SPAN_OF_CHAR
#endif
using System;

namespace Smdn.Formats.Mime;

#pragma warning disable IDE0040
static partial class ContentTransferEncoding {
#pragma warning restore IDE0040
  private static string ValidateEncodingMethodString(string input, string paramName)
  {
    if (input is null)
      throw new ArgumentNullException(paramName);
    if (input.Length == 0)
      throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(paramName);

    return input;
  }

  public static ContentTransferEncodingMethod Parse(string str)
    => TryParse(ValidateEncodingMethodString(str, nameof(str)), out var encoding)
      ? encoding
      : throw new FormatException($"unsupported content transfer encoding: '{str}'");

  // TODO: public static ContentTransferEncodingMethod Parse(ReadOnlySpan<char> str)

  public static bool TryParse(string str, out ContentTransferEncodingMethod encoding)
  {
    encoding = ContentTransferEncodingMethod.Unknown;

    if (string.IsNullOrEmpty(str))
      return false;

    return contentTransferEncodingMethods.TryGetValue(str, out encoding);
  }

  // TODO: public static bool TryParse(ReadOnlySpan<char> str, out ContentTransferEncodingMethod encoding)

  public static bool TryFormat(
    ContentTransferEncodingMethod encoding,
    Span<char> destination,
    out int charsWritten
  )
  {
    charsWritten = 0;

    var str = encoding switch {
      ContentTransferEncodingMethod.SevenBit => ContentTransferEncodingMethodStringSevenBit,
      ContentTransferEncodingMethod.EightBit => ContentTransferEncodingMethodStringEightBit,
      ContentTransferEncodingMethod.Binary => ContentTransferEncodingMethodStringBinary,
      ContentTransferEncodingMethod.Base64 => ContentTransferEncodingMethodStringBase64,
      ContentTransferEncodingMethod.QuotedPrintable => ContentTransferEncodingMethodStringQuotedPrintable,
      ContentTransferEncodingMethod.UUEncode => ContentTransferEncodingMethodStringUUEncode,
      ContentTransferEncodingMethod.GZip64 => ContentTransferEncodingMethodStringGZip64,
      _ => null,
    };

    if (str is null)
      return false;
    if (destination.Length < str.Length)
      return false;

#if SYSTEM_STRING_COPYTO_SPAN_OF_CHAR
    str.CopyTo(destination);
#else
    str.AsSpan().CopyTo(destination);
#endif

    charsWritten = str.Length;

    return true;
  }
}
