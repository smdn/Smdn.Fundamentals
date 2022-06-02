// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.Mime;

#pragma warning disable IDE0040
static partial class ContentTransferEncoding {
#pragma warning restore IDE0040
  [Obsolete($"Use {nameof(TryParse)}() instead.")]
  public static ContentTransferEncodingMethod GetEncodingMethod(string encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    if (contentTransferEncodingMethods.TryGetValue(encoding, out var method))
      return method;
    else
      return ContentTransferEncodingMethod.Unknown;
  }

  [Obsolete($"Use {nameof(Parse)}() instead.")]
  public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding)
  {
    var ret = GetEncodingMethod(encoding);

    if (ret == ContentTransferEncodingMethod.Unknown)
      throw new NotSupportedException($"unsupported content transfer encoding: '{encoding}'");

    return ret;
  }

  public static string GetEncodingName(ContentTransferEncodingMethod method)
    => method switch {
      ContentTransferEncodingMethod.SevenBit => ContentTransferEncodingMethodStringSevenBit,
      ContentTransferEncodingMethod.EightBit => ContentTransferEncodingMethodStringEightBit,
      ContentTransferEncodingMethod.Binary => ContentTransferEncodingMethodStringBinary,
      ContentTransferEncodingMethod.Base64 => ContentTransferEncodingMethodStringBase64,
      ContentTransferEncodingMethod.QuotedPrintable => ContentTransferEncodingMethodStringQuotedPrintable,
      ContentTransferEncodingMethod.UUEncode => ContentTransferEncodingMethodStringUUEncode,
      ContentTransferEncodingMethod.GZip64 => ContentTransferEncodingMethodStringGZip64,
      _ => throw ExceptionUtils.CreateNotSupportedEnumValue(method),
    };
}
