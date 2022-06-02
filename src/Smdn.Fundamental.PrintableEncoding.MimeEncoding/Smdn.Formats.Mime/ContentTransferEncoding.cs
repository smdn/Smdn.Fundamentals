// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Formats.Mime;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class ContentTransferEncoding {
  public const string HeaderName = "Content-Transfer-Encoding";

  private const string ContentTransferEncodingMethodStringSevenBit = "7bit";
  private const string ContentTransferEncodingMethodStringEightBit = "8bit";
  private const string ContentTransferEncodingMethodStringBinary = "binary";
  private const string ContentTransferEncodingMethodStringBase64 = "base64";
  private const string ContentTransferEncodingMethodStringQuotedPrintable = "quoted-printable";
  private const string ContentTransferEncodingMethodStringUUEncode = "x-uuencode";
  private const string ContentTransferEncodingMethodStringGZip64 = "x-gzip64";

  private static readonly IReadOnlyDictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods
    = new Dictionary<string, ContentTransferEncodingMethod>(StringComparer.OrdinalIgnoreCase) {
      // standards
      { ContentTransferEncodingMethodStringSevenBit,        ContentTransferEncodingMethod.SevenBit },
      { ContentTransferEncodingMethodStringEightBit,        ContentTransferEncodingMethod.EightBit },
      { ContentTransferEncodingMethodStringBinary,          ContentTransferEncodingMethod.Binary },
      { ContentTransferEncodingMethodStringBase64,          ContentTransferEncodingMethod.Base64 },
      { ContentTransferEncodingMethodStringQuotedPrintable, ContentTransferEncodingMethod.QuotedPrintable },

      // non-standards
      { ContentTransferEncodingMethodStringUUEncode,        ContentTransferEncodingMethod.UUEncode },
      { "x-uuencoded",                                      ContentTransferEncodingMethod.UUEncode },
      { "x-uu",                                             ContentTransferEncodingMethod.UUEncode },
      { "x-uue",                                            ContentTransferEncodingMethod.UUEncode },
      { "uuencode",                                         ContentTransferEncodingMethod.UUEncode },
      { ContentTransferEncodingMethodStringGZip64,          ContentTransferEncodingMethod.GZip64 },
      { "gzip64",                                           ContentTransferEncodingMethod.GZip64 },
    };
}
