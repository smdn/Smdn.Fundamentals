// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.Mime;

#pragma warning disable IDE0040
static partial class ContentTransferEncoding {
#pragma warning restore IDE0040
  public static ContentTransferEncodingMethod GetEncodingMethod(string encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    if (contentTransferEncodingMethods.TryGetValue(encoding, out var method))
      return method;
    else
      return ContentTransferEncodingMethod.Unknown;
  }

  public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding)
  {
    var ret = GetEncodingMethod(encoding);

    if (ret == ContentTransferEncodingMethod.Unknown)
      throw new NotSupportedException($"unsupported content transfer encoding: '{encoding}'");

    return ret;
  }

  public static string GetEncodingName(ContentTransferEncodingMethod method)
    => method switch {
      ContentTransferEncodingMethod.SevenBit => "7bit",
      ContentTransferEncodingMethod.EightBit => "8bit",
      ContentTransferEncodingMethod.Binary => "binary",
      ContentTransferEncodingMethod.Base64 => "base64",
      ContentTransferEncodingMethod.QuotedPrintable => "quoted-printable",
      ContentTransferEncodingMethod.UUEncode => "x-uuencode",
      ContentTransferEncodingMethod.GZip64 => "x-gzip64",
      _ => throw ExceptionUtils.CreateNotSupportedEnumValue(method),
    };
}
